using AOIClient.Internal;
using Kosher.Sockets;
using Kosher.Sockets.Interface;
using Protocol.SAndC;
using Share;
using System.Text.Json;

namespace AOIClient.Modules.Handler
{
    public partial class SCProtocolHandler : ISessionComponent
    {
        public Session Session { get; private set; }
        public void Dispose()
        {
            Session = null;
        }

        public void SetSession(Session session)
        {
            Session = session;
        }
        public void Process(ConnectResponse body)
        {
            var player = new Player()
            {
                Nickname = body.PlyerName,
                CellPos = new Vector2Int(body.X, body.Y)
            };
            GameManager.Instance.SetUserPlayer(player);
        }
        public void Process(Spawn body)
        {
            GameManager.Instance.Players.Add(body.Player);
        }
        public void Process(Despawn body)
        {
            GameManager.Instance.Players.Remove(body.Player);
        }
        public void Process(MoveResponse body)
        {
            GameManager.Instance.UserPlayer.CellPos += new Vector2Int(body.X, body.Y);

            //GameManager.Instance.EnterMyPlayer();
        }
    }
}
