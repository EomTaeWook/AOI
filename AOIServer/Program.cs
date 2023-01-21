// See https://aka.ms/new-console-template for more information

using AOIServer.Modules;
using CLISystem;
using Kosher.Extensions.Log;
using Kosher.Log;

var logConfigPath = $"{AppContext.BaseDirectory}/KosherLog.config";

LogBuilder.Configuration(LogConfigXmlReader.Load(logConfigPath));
LogBuilder.Build();

CLIModule cliModule = new();

cliModule.Build();
cliModule.Run();

AOIServerModule.Instance.Start();