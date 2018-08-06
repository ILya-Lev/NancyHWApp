using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ShoppingCart461.Startup))]

namespace ShoppingCart461
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //todo: is not working!
            //app.Use(typeof(MonitoringMiddleware), new StateValidator(1));
            //app.Use<MonitoringMiddleware>(new StateValidator(1));
            app.UseNancy();
        }
    }
}