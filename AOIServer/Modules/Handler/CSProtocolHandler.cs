using AOIServer.Internal;
using AOIServer.Net;
using Dignus.Sockets;
using Dignus.Sockets.Attribute;
using Dignus.Sockets.Extensions;
using Dignus.Sockets.Interface;
using Protocol.CAndS;
using Protocol.SAndC;
using Share;
using System.Text.Json;

namespace AOIServer.Modules.Handler
{
    public partial class CSProtocolHandler : IProtocolHandler<string>, ISessionComponent
    {
        private User User { get; set; }
        public Session Session { get; private set; }

        [ProtocolName("Move")]
        public void Process(Move packet)
        {
            var targetPosition = User.Player.CellPos + new Vector2Int(packet.X, packet.Y);

            var result = GameManager.Instance.Move(User,
                targetPosition);

            Session.Send(Packet.MakePacket(SCProtocol.MoveResponse,
                new MoveResponse()
                {
                    Ok = result,
                    X = packet.X,
                    Y = packet.Y
                }));

            GameManager.Instance.UpdateAroundPlayer(User);

        }
        public void Login(Login packet)
        {
            Player player;
            if (packet.IsNpc == true)
            {
                player = new Player()
                {
                    CellPos = new Vector2Int(250, 70),
                    Nickname = $"Player{Session.Id}",
                };
            }
            else
            {
                player = new Player()
                {
                    CellPos = new Vector2Int(150, 70),
                    Nickname = $"Player{Session.Id}",
                };
            }

            this.User = new User(player, Session);

            Session.Send(Packet.MakePacket(SCProtocol.LoginResponse,
                new LoginResponse()
                {
                    IsNpc = packet.IsNpc,
                    Player = player
                }));

            GameManager.Instance.EnterGame(User);
            GameManager.Instance.UpdateAroundPlayer(User);


        }
        public void SetSession(Session session)
        {
            Session = session;
        }

        public void Dispose()
        {
            if (User == null)
            {
                return;
            }
            var packet = Packet.MakePacket<Despawn>(SCProtocol.Despawn, new Despawn()
            {
                Player = User.Player
            });

            foreach (var item in User.AroundPlayers)
            {
                item.Session.Send(packet);
                item.AroundPlayers.Remove(User);
            }
            GameManager.Instance.LeaveGame(User);
        }

        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
        }
    }
}
