using AOIServer.Net.Interface;
using Kosher.Log;
using Kosher.Sockets;
using Kosher.Sockets.Interface;
using Protocol.SAndC;
using System.Text.Json;

namespace AOIServer.Modules
{
    public class AOIHandler : ISessionComponent
    {
        public Session Session { get; private set; }
        public void Process(SAndCProtocol protocol, string body)
        {
            if (CheckProtocol(protocol) == false)
            {
                LogHelper.Error($"not found callback - {protocol}");
                return;
            }
            Execute(protocol, body);
        }

        public bool CheckProtocol(SAndCProtocol protocol)
        {
            if(protocol == SAndCProtocol.None ||
                protocol == SAndCProtocol.Max)
            {
                return false;
            }

            return true;
        }
        public void Execute(SAndCProtocol protocol, string body)
        {

        }

        private void Move(string body)
        {
            var packetData = JsonSerializer.Deserialize<Move>(body);
            Process(packetData);
        }
        
        public void SetSession(Session session)
        {
            Session = session;
        }

        public void Dispose()
        {
            
        }

        public Task Process(Move move)
        {
            return Task.CompletedTask;
        }
    }
}
