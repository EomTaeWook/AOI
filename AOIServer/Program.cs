// See https://aka.ms/new-console-template for more information

using AOIServer.Modules;
using AOIServer.Modules.Handler;
using Kosher.Extensions.Log;
using Kosher.Log;

var logConfigPath = $"{AppContext.BaseDirectory}/KosherLog.config";

LogBuilder.Configuration(LogConfigXmlReader.Load(logConfigPath));
LogBuilder.Build();

CSProtocolHandler.Init();

AOIServerModule.Instance.Start();