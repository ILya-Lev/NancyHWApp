using LibOwin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NancyHWApp
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class ConsoleMiddleware
    {
        private readonly AppFunc _next;

        public ConsoleMiddleware(AppFunc next)
        {
            _next = next;
        }

        public async Task Run(IDictionary<string, object> environment)
        {
            var context = new OwinContext(environment);
            var method = context.Request.Method;
            var path = context.Request.Path;

            Console.WriteLine($"Got request: {method} {path}");
            await _next(environment);
            Console.WriteLine($"Got response: {context.Response.StatusCode}");
        }
    }
}
