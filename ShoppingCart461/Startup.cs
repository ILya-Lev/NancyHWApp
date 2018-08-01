using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ShoppingCart461.Startup))]

namespace ShoppingCart461
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}