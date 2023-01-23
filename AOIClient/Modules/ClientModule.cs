using AOIClient.Modules.Serializer;
using Kosher.Framework;
using Kosher.Log;
using Kosher.Sockets;
using Kosher.Sockets.Interface;

namespace AOIClient.Modules
{
    internal class AOIClient : BaseClient
    {
        public AOIClient(SessionCreator sessionCreator) : base(sessionCreator)
        {

        }

        protected override void OnConnected(Session session)
        {
            LogHelper.Debug($"[Client] Server Connected");
        }

        protected override void OnDisconnected(Session session)
        {
            LogHelper.Debug($"[Client] Server Disconnected");
        }
    }

    internal class ClientModule : Singleton<ClientModule>
    {
        readonly AOIClient _client;
        public bool IsConnected = false;
        public ClientModule()
        {
            _client = new AOIClient(new SessionCreator(MakeSerializersFunc));
        }
        public void Connect()
        {
            if(IsConnected == true)
            {
                return;
            }
            IsConnected = true;
            try
            {
                _client.Connect("127.0.0.1", 31000);
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
                IsConnected = false;
            }
            
        }

        private Tuple<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>> MakeSerializersFunc()
        {
            return Tuple.Create<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>>(
                new PacketSerializer(),
                new PacketDeserializer(),
                new List<ISessionComponent>() { }
                );
        }
    }

}
