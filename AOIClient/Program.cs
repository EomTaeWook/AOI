using AOIClient.Modules;
using AOIClient.Modules.Handler;
using Kosher.Extensions.Log;
using Kosher.Log;
using Kosher.Sockets;
using Protocol.SAndC;

namespace AOIClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.


            LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}KosherLog.config"));
            LogBuilder.Build();

            HandlerBinder<SCProtocolHandler>.Bind<SCProtocol, string>();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}