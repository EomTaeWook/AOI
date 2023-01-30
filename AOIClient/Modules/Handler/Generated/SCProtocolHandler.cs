using Kosher.Log;

namespace AOIClient.Modules.Handler
{
    public partial class SCProtocolHandler
    {
        private static Action<SCProtocolHandler, string>[] _handlers;
        public static void Init()
        {
            _handlers = new Action<SCProtocolHandler, string>[4];
            _handlers[0] = (t, body) => t.ProcessConnectResponse(body);
            _handlers[1] = (t, body) => t.ProcessMoveResponse(body);
            _handlers[2] = (t, body) => t.ProcessSpawn(body);
            _handlers[3] = (t, body) => t.ProcessDespawn(body);
        }
        public static bool CheckProtocol(int protocol)
        {
            if(protocol < 0 && protocol >= _handlers.Length)
            {
                return false;
            }
            return true;
        }
        public void Process(int protocol, string body)
        {
            _handlers[protocol](this, body);
        }
        protected void ProcessConnectResponse(string body)
        {
            if(body == null)
            {
                LogHelper.Error("body is null");
                return;
            }
            var packet = DeserializeBody<Protocol.SAndC.ConnectResponse>(body);
            Process(packet);
        }
        protected void ProcessMoveResponse(string body)
        {
            if(body == null)
            {
                LogHelper.Error("body is null");
                return;
            }
            var packet = DeserializeBody<Protocol.SAndC.MoveResponse>(body);
            Process(packet);
        }
        protected void ProcessSpawn(string body)
        {
            if(body == null)
            {
                LogHelper.Error("body is null");
                return;
            }
            var packet = DeserializeBody<Protocol.SAndC.Spawn>(body);
            Process(packet);
        }
        protected void ProcessDespawn(string body)
        {
            if(body == null)
            {
                LogHelper.Error("body is null");
                return;
            }
            var packet = DeserializeBody<Protocol.SAndC.Despawn>(body);
            Process(packet);
        }
        public T DeserializeBody<T>(string body)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(body);
        }
    }
}
