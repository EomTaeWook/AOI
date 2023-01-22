using AOIServer.Modules.Handler;
using AOIServer.Modules.Serializer;
using Kosher.Framework;
using Kosher.Log;
using Kosher.Sockets;
using Kosher.Sockets.Interface;

namespace AOIServer.Modules
{
    public class AOIServer : BaseServer
    {
        public AOIServer(SessionCreator sessionCreator) : base(sessionCreator)
        {
        }

        protected override void OnAccepted(Session session)
        {
            LogHelper.Info($"[Server] acceptd session : {session.Id}");

        }

        protected override void OnDisconnected(Session session)
        {
            LogHelper.Info($"[Server] disconnected : {session.Id}");
        }
    }

    public class AOIServerModule : Singleton<AOIServerModule>
    {
        private readonly AOIServer _aoiServer;
        private bool _isActive = false;
        public AOIServerModule()
        {
            _aoiServer = new AOIServer(new SessionCreator(MakeSerializersFunc));

            SAndCProtocolHandler.Init();
        }
        public void Start()
        {
            int port = 10000;
            Task.Run(async () =>
            {
                _aoiServer.Start(port);
                LogHelper.Debug($"aoi server start... port : {port}");
                _isActive = true;
                while (_isActive)
                {
                    await Task.Delay(33);
                }
            }).GetAwaiter().GetResult();
        }
        private Tuple<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>> MakeSerializersFunc()
        {
            SAndCProtocolHandler handler = new();

            return Tuple.Create<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>>(
                new PacketSerializer(),
                new PacketDeserializer(handler),
                new List<ISessionComponent>() { }
                );
        }

    }
}
