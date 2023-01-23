using AOIClient.Modules;
using Kosher.Extensions.Log;
using Kosher.Log;

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

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}