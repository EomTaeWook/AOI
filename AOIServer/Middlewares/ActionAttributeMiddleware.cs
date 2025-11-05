using AOIServer.Attributes;
using Dignus.Framework.Pipeline;
using Dignus.Framework.Pipeline.Interfaces;

namespace AOIServer.Middlewares
{
    internal class ActionAttributeMiddleware : IAsyncMiddleware<PipeContext>
    {
        private readonly List<ActionAttribute> _actionAttributes;
        public ActionAttributeMiddleware(List<ActionAttribute> actionAttributes)
        {
            _actionAttributes = actionAttributes;
        }

        public Task InvokeAsync(ref PipeContext context, ref AsyncPipelineNext<PipeContext> next)
        {
            foreach (var actionAttribute in _actionAttributes)
            {
                if (actionAttribute.ActionExecute(context) == false)
                {
                    return Task.CompletedTask;
                }
            }
            return next.InvokeAsync(ref context);
        }
    }
}
