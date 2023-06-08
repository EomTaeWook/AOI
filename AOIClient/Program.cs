using AOIClient.Modules.Handler;
using Dignus.Extensions.Log;
using Dignus.Log;
using Dignus.Sockets;
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


            LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
            LogBuilder.Build();

            HandlerBinder<SCProtocolHandler, string>.Bind<SCProtocol>();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}