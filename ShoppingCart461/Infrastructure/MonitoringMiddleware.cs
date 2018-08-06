using LibOwin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart461.Infrastructure
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MonitoringMiddleware
    {
        private readonly AppFunc _next;
        private readonly IStateValidator _stateValidator;

        private static readonly PathString _shallowSegment = new PathString("/_monitoring/shallow");
        private static readonly PathString _deepSegment = new PathString("/_monitoring/deep");

        public MonitoringMiddleware(AppFunc next, IStateValidator stateValidator)
        {
            _next = next;
            _stateValidator = stateValidator;
        }

        public async Task Run(IDictionary<string, object> environment)
        {
            var context = new OwinContext(environment);

            if (context.Request.Path.Equals(_shallowSegment, StringComparison.OrdinalIgnoreCase))
            {
                await ShallowCheck(context);
            }
            else if (context.Request.Path.Equals(_deepSegment, StringComparison.OrdinalIgnoreCase))
            {
                await DeepCheck(context);
            }
            else
            {
                await _next(environment);
            }
        }

        private async Task DeepCheck(OwinContext context)
        {
            if (await _stateValidator.IsValidState())
                context.Response.StatusCode = 204;  //no content
            else
                context.Response.StatusCode = 503;  //service unavailable
        }

        private Task ShallowCheck(OwinContext context)
        {
            context.Response.StatusCode = 204;  //no content
            return Task.FromResult(0);
        }
    }
}