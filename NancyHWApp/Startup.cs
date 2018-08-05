using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NancyHWApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOwin(buildFunc => NancyPipelineSetup(buildFunc));
        }

        private static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> NancyPipelineSetup(Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> buildFunc)
        {
            buildFunc(next => async (env) =>
            {
                Debug.WriteLine($"got request {env["owin.RequestPath"]}");
                await next(env);
                Debug.WriteLine($"processing response {env["owin.ResponseStatusCode"]}");
            });

            buildFunc(next => new ConsoleMiddleware(next).Run);

            return buildFunc.UseNancy();
        }
    }
}
