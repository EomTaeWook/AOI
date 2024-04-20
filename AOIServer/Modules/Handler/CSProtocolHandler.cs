using AOIServer.Internal;
using AOIServer.Net;
using Dignus.Sockets.Attributes;
using Dignus.Sockets.Interfaces;
using Protocol.CAndS;
using Protocol.SAndC;
using Share;
using System.Text.Json;

namespace AOIServer.Modules.Handler
{
    public partial class CSProtocolHandler : IProtocolHandler<string>, ISessionHandler
    {
        private User User { get; set; }
        public ISession Session { get; private set; }

        [ProtocolName("Move")]
        public void Process(Move packet)
        {
            var targetPosition = User.Player.CellPos + new Vector2Int(packet.X, packet.Y);

            var result = GameManager.Instance.Move(User,
                targetPosition);

            if (result == false)
            {
                Session.Send(Packet.MakePacket(SCProtocol.MoveResponse,
                new MoveResponse()
                {
                    Ok = false
                }));
            }
            else
            {
                Session.Send(Packet.MakePacket(SCProtocol.MoveResponse,
                new MoveResponse()
                {
                    Ok = result,
                    X = packet.X,
                    Y = packet.Y
                }));
                GameManager.Instance.UpdateAroundPlayer(User);
            }
        }
        public void Login(Login packet)
        {
            Player player;
            if (packet.IsNpc == true)
            {
                var x = Random.Shared.Next(0, GameManager.Instance.GetMap().GetMaxX());
                var y = Random.Shared.Next(0, GameManager.Instance.GetMap().GetMaxY());
                player = new Player()
                {
                    CellPos = new Vector2Int(x, y),
                    Nickname = packet.Nickname,
                };
            }
            else
            {
                player = new Player()
                {
                    CellPos = new Vector2Int(0, 0),
                    Nickname = packet.Nickname,
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
        public void SetSession(ISession session)
        {
            Session = session;
        }

        public void Dispose()
        {
            if (User == null)
            {
                return;
            }
            GameManager.Instance.LeaveGame(User);
        }

        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
        }
    }
}
