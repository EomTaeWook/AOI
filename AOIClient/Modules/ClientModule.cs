using AOIClient.Modules.Handler;
using AOIClient.Modules.Serializer;
using AOIClient.Net;
using Kosher.Framework;
using Kosher.Log;
using Kosher.Sockets;
using Kosher.Sockets.Interface;
using Protocol.CAndS;

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
        private List<AOIClient> _clients = new List<AOIClient>();
        public bool IsConnected = false;

        int index = 0;
        public ClientModule()
        {
            _client = new AOIClient(new SessionCreator(MakeSerializersFunc));
            
        }
        public void AddNpc()
        {
            var npc = new AOIClient(new SessionCreator(MakeSerializersFunc));
            try
            {
                npc.Connect("127.0.0.1", 10000);
                _clients.Add(npc);
                npc.Send(Packet.MakePacket<Login>(CSProtocol.Login, new Login()
                {
                    IsNpc = true,
                }));
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
            }
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
                _client.Connect("127.0.0.1", 10000);

                _client.Send(Packet.MakePacket<Login>(CSProtocol.Login, new Login()
                {
                }));
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
                IsConnected = false;
            }
        }

        private Tuple<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>> MakeSerializersFunc()
        {
            SCProtocolHandler handler = new SCProtocolHandler();
            return Tuple.Create<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>>(
                new PacketSerializer(),
                new PacketDeserializer(handler),
                new List<ISessionComponent>() 
                {
                    handler 
                });
        }

        public void Send(IPacket packet)
        {
            _client.Send(packet);
        }
    }

}
