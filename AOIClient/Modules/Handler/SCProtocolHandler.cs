using AOIClient.Internal;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Attribute;
using Dignus.Sockets.Interface;
using Protocol.SAndC;
using Share;
using System.Text.Json;

namespace AOIClient.Modules.Handler
{
    public partial class SCProtocolHandler : IProtocolHandler<string>, ISessionComponent
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
        [ProtocolName("LoginResponse")]
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
        [ProtocolName("Spawn")]
        public void Process(Spawn body)
        {
            if(body.Player.Nickname != GameManager.Instance.UserPlayer.Nickname)
            {
                if(GameManager.Instance.AddPlayer(body.Player) == false)
                {
                    LogHelper.Error($"duplicated player player Id : {body.Player.Nickname}"); 
                }
            }
        }
        [ProtocolName("Despawn")]
        public void Process(Despawn body)
        {
            if (body.Player.Nickname != GameManager.Instance.UserPlayer.Nickname)
            {
                GameManager.Instance.RemovePlayer(body.Player);
            }
        }
        [ProtocolName("MoveResponse")]
        public void Process(MoveResponse body)
        {
            GameManager.Instance.UserPlayer.CellPos += new Vector2Int(body.X, body.Y);

            //GameManager.Instance.EnterMyPlayer();
        }

        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
        }
    }
}
