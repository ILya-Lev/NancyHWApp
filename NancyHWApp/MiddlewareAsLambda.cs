using LibOwin;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("NancyHWApp.Tests")]

namespace NancyHWApp
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MiddlewareAsLambda
    {
        public Func<AppFunc, AppFunc> Impl = next => async env =>
        {
            var context = new OwinContext(env);
            if (context.Request.Path.Value == @"/test/path")
                context.Response.StatusCode = 404;
            else
                await next(env);
        };
    }
}
