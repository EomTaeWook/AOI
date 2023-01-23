using Kosher.Sockets;
using Kosher.Sockets.Interface;
using Protocol.SAndC;
using System.Text.Json;

namespace AOIServer.Modules.Handler
{
    public partial class SAndCProtocolHandler : ISessionComponent
    {
        public Session Session { get; private set; }
        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
        }
        
        public void Process(LoginResponse packet)
        {

        }
        public void SetSession(Session session)
        {
            Session = session;
        }

        public void Dispose()
        {

        }
    }
}
