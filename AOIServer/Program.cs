// See https://aka.ms/new-console-template for more information

using AOIServer.Attributes;
using AOIServer.Middlewares;
using AOIServer.Modules;
using AOIServer.Modules.Handler;
using Dignus.Log;
using Dignus.Sockets;
using Protocol.CAndS;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var logConfigPath = $"{AppContext.BaseDirectory}/DignusLog.config";

        LogBuilder.Configuration(LogConfigXmlReader.Load(logConfigPath));
        LogBuilder.Build();

        var invoker = ProtocolHandlerMapper<CSProtocolHandler, string>.BindAndCreateInvoker<PipeContext, CSProtocol>();

        ProtocolPipelineInvoker<PipeContext, CSProtocolHandler, string>.Use<CSProtocol>(invoker,
            (method, pipeline) =>
            {
                var filters = method.GetCustomAttributes<ActionAttribute>();
                var orderedFilters = filters.OrderBy(r => r.Order).ToList();
                if (orderedFilters.Count > 0)
                {
                    var actionMiddleware = new ActionAttributeMiddleware(orderedFilters);
                    pipeline.Use(actionMiddleware);
                }
            });

        AOIServerModule.Instance.Start();
    }
}