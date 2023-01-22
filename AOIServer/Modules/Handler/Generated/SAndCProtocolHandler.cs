using Kosher.Log;

namespace AOIServer.Modules.Handler
{
    public interface ISAndCProtocolHandler
    {
        public T DeserializeBody<T>(string body);
    }
    public partial class SAndCProtocolHandler : ISAndCProtocolHandler
    {
        private static Action<SAndCProtocolHandler, string>[] _handlers;
        public static void Init()
        {
            _handlers = new Action<SAndCProtocolHandler, string>[1];
            _handlers[0] = (t, body) => t.ProcessMove(body);
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
        protected void ProcessMove(string body)
        {
            if(body == null)
            {
                LogHelper.Error("body is null");
                return;
            }
            var packet = DeserializeBody<Protocol.SAndC.Move>(body);
            Process(packet);
        }
    }
}
