﻿using AOIServer.Modules.Handler;
using AOIServer.Modules.Serializer;
using Dignus.Framework;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interface;

namespace AOIServer.Modules
{
    public class AOIServer : ServerBase
    {
        public AOIServer(SessionCreator sessionCreator) : base(sessionCreator)
        {
        }

        protected override void OnAccepted(Session session)
        {
            LogHelper.Info($"[Server] accepted session : {session.Id}");
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
        private Tuple<IPacketSerializer, IPacketDeserializer, ICollection<ISessionHandler>> MakeSerializersFunc()
        {
            CSProtocolHandler handler = new();

            return Tuple.Create<IPacketSerializer, IPacketDeserializer, ICollection<ISessionHandler>>(
                new PacketSerializer(),
                new PacketDeserializer(handler),
                new List<ISessionHandler>()
                {
                    handler
                });
        }
    }
}
