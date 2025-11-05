using AOIServer.Middlewares;

namespace AOIServer.Attributes
{
    internal class AuthorizationAttribute : ActionAttribute
    {
        public override bool ActionExecute(PipeContext context)
        {
            return context.Handler?.User != null;
        }
    }
}
