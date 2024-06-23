using AOIServer.Modules.Handler;
using AOIServer.Modules.Serializer;
using Dignus.Framework;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using System;
using System.Collections.Generic;

namespace AOIServer.Modules
{
    public class AOIServer : ServerBase
    {
        public AOIServer(SessionConfiguration sessionConfiguration) : base(sessionConfiguration)
        {
        }

        protected override void OnAccepted(ISession session)
        {
            LogHelper.Info($"[Server] accepted session : {session.Id}");
        }

        protected override void OnDisconnected(ISession session)
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
            _aoiServer = new AOIServer(new SessionConfiguration(MakeSerializersFunc));
        }
        public void Start()
        {
            int port = 10000;
            Task.Run(async () =>
            {
                _aoiServer.Start("", port, ProtocolType.Tcp, 100);
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
            CSProtocolHandler handler = new();

            return Tuple.Create<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>>(
                new PacketSerializer(),
                new PacketDeserializer(handler),
                new List<ISessionComponent>()
                {
                    handler
                });
        }
    }
}
