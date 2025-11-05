using AOIClient.Modules.Handler;
using AOIClient.Modules.Serializer;
using AOIClient.Net;
using Dignus.Framework;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using Protocol.CAndS;

namespace AOIClient.Modules
{
    internal class AOIClient : ClientBase
    {
        public AOIClient(SessionConfiguration sessionConfiguration) : base(sessionConfiguration)
        {
        }

        protected override void OnConnected(ISession session)
        {
            LogHelper.Debug($"[Client] Server Connected");
        }

        protected override void OnDisconnected(ISession session)
        {
            LogHelper.Debug($"[Client] Server Disconnected");
        }
    }

    internal class ClientModule : Singleton<ClientModule>
    {
        readonly AOIClient _client;
        private List<AOIClient> _npcs = new List<AOIClient>();
        public bool IsConnected = false;
        private static volatile int _npcNumber = 0;
        public ClientModule()
        {
            _client = new AOIClient(new SessionConfiguration(MakeSerializersFunc));

        }
        public void AddNpc()
        {
            var npc = new AOIClient(new SessionConfiguration(MakeSerializersFunc));
            try
            {
                npc.Connect("127.0.0.1", 10000);
                _npcs.Add(npc);
                npc.Send(Packet.MakePacket<Login>(CSProtocol.Login, new Login()
                {
                    IsNpc = true,
                    Nickname = $"Npc{_npcNumber++}"
                }));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        public void Connect()
        {
            if (IsConnected == true)
            {
                return;
            }
            IsConnected = true;
            try
            {
                _client.Connect("127.0.0.1", 10000);

                _client.Send(Packet.MakePacket(CSProtocol.Login,
                    new Login()
                    {
                        Nickname = "Player1"
                    }));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                IsConnected = false;
            }
        }

        private SessionSetup MakeSerializersFunc()
        {
            SCProtocolHandler handler = new SCProtocolHandler();
            return new SessionSetup(
                new PacketSerializer(),
                new PacketProcessor(handler),
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
