// See https://aka.ms/new-console-template for more information

using AOIServer.Modules;
using AOIServer.Modules.Handler;
using Dignus.Extensions.Log;
using Dignus.Log;
using Dignus.Sockets;
using Protocol.CAndS;

internal class Program
{
    private static void Main(string[] args)
    {
        var logConfigPath = $"{AppContext.BaseDirectory}/DignusLog.config";

        LogBuilder.Configuration(LogConfigXmlReader.Load(logConfigPath));
        LogBuilder.Build();

        HandlerBinder<CSProtocolHandler>.BindProtocol<CSProtocol>();

        AOIServerModule.Instance.Start();
    }
}