using AOIServer.Modules.Handler;
using Dignus.Sockets.Interfaces;
using Dignus.Sockets.Pipeline;

namespace AOIServer.Middlewares
{
    public struct PipeContext : IPipelineContext<CSProtocolHandler, string>
    {
        public ISession Session { get; init; }
        public int Protocol { get; set; }

        public CSProtocolHandler Handler { get; set; }

        public string Body { get; set; }
    }
}
