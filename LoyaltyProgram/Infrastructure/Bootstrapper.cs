using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.Diagnostics;

namespace LoyaltyProgram.Infrastructure
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
            => NancyInternalConfiguration.WithOverrides(config => config.StatusCodeHandlers.Clear());

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.OnError += (ctx, exc) =>
            {
                Log("Unhandled", exc);
                return null;
            };
        }

        private void Log(string message, Exception exc)
        {
            Debug.Print($"{message}; {exc.Message}");
        }
    }
}
