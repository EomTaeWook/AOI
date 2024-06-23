using AOIClient.Internal;
using Dignus.Log;
using Dignus.Sockets.Attributes;
using Dignus.Sockets.Interfaces;
using Protocol.SAndC;
using ShareModel;
using System.Text.Json;

namespace AOIClient.Modules.Handler
{
    public partial class SCProtocolHandler : IProtocolHandler<string>, ISessionComponent
    {
        public ISession Session { get; private set; }
        private bool _isNpc;
        public void Dispose()
        {
            Session = null;
        }

        public void SetSession(ISession session)
        {
            Session = session;
        }
        [ProtocolName("LoginResponse")]
        public void LoginResponse(LoginResponse body)
        {
            if (body.IsNpc == false)
            {
                GameManager.Instance.SetUserPlayer(body.Player);
            }
            _isNpc = body.IsNpc;
        }
        [ProtocolName("Spawn")]
        public void Process(Spawn body)
        {
            if (_isNpc == true)
            {
                return;
            }
            if (body.Player.Nickname != GameManager.Instance.UserPlayer.Nickname)
            {
                if (GameManager.Instance.AddPlayer(body.Player) == false)
                {
                    LogHelper.Error($"duplicated player player Id : {body.Player.Nickname}");
                }
            }
        }
        [ProtocolName("Despawn")]
        public void Process(Despawn body)
        {
            if (_isNpc == true)
            {
                return;
            }
            if (body.Player.Nickname != GameManager.Instance.UserPlayer.Nickname)
            {
                GameManager.Instance.RemovePlayer(body.Player);
            }
        }
        [ProtocolName("MoveResponse")]
        public void Process(MoveResponse body)
        {
            var position = new Vector2Int(body.X, body.Y);
            GameManager.Instance.UserPlayer.CellPos += position;
        }

        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
        }
    }
}
