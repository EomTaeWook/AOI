// See https://aka.ms/new-console-template for more information

using AOIServer.Modules;
using AOIServer.Modules.Handler;
using Kosher.Extensions.Log;
using Kosher.Log;
using Kosher.Sockets;
using Kosher.Sockets.Interface;
using Protocol.CAndS;
using Protocol.SAndC;

internal class Program
{
    private static void Main(string[] args)
    {
        var logConfigPath = $"{AppContext.BaseDirectory}/KosherLog.config";

        LogBuilder.Configuration(LogConfigXmlReader.Load(logConfigPath));
        LogBuilder.Build();

        HandlerBinder<CSProtocolHandler>.Bind<CSProtocol, string>();

        AOIServerModule.Instance.Start();
    }
}