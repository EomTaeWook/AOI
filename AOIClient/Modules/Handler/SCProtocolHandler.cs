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
        public void Process(LoginResponse body)
        {
            if (body.IsNpc)
            {
                //GameManager.Instance.Players.Add(body.Player);
            }
            else
            {
                GameManager.Instance.SetUserPlayer(body.Player);
            }

        }
        public void Process(Spawn body)
        {
            if(body.Player.Nickname != GameManager.Instance.UserPlayer.Nickname)
            {
                GameManager.Instance.Players.Add(body.Player);
            }
        }
        public void Process(Despawn body)
        {
            if (body.Player.Nickname != GameManager.Instance.UserPlayer.Nickname)
            {
                GameManager.Instance.Players.Remove(body.Player);
            }
        }
        public void Process(MoveResponse body)
        {
            GameManager.Instance.UserPlayer.CellPos += new Vector2Int(body.X, body.Y);

            //GameManager.Instance.EnterMyPlayer();
        }
    }
}
