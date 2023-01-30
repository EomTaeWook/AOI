using Kosher.Log;

namespace AOIServer.Modules.Handler
{
    public partial class CSProtocolHandler
    {
        private static Action<CSProtocolHandler, string>[] _handlers;
        public static void Init()
        {
            _handlers = new Action<CSProtocolHandler, string>[2];
            _handlers[0] = (t, body) => t.ProcessLogin(body);
            _handlers[1] = (t, body) => t.ProcessMove(body);
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
        protected void ProcessLogin(string body)
        {
            if(body == null)
            {
                LogHelper.Error("body is null");
                return;
            }
            var packet = DeserializeBody<Protocol.CAndS.Login>(body);
            Process(packet);
        }
        protected void ProcessMove(string body)
        {
            if(body == null)
            {
                LogHelper.Error("body is null");
                return;
            }
            var packet = DeserializeBody<Protocol.CAndS.Move>(body);
            Process(packet);
        }
        public T DeserializeBody<T>(string body)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(body);
        }
    }
}
