using FluentAssertions;
using LibOwin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NancyHWApp.Tests
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MiddlewareAsLambdaTests
    {
        private static readonly AppFunc _noOperation = env => Task.FromResult(0);

        [Fact]
        public void Impl_TestPath_404()
        {
            var context = new OwinContext();
            context.Request.Path = new PathString("/test/path");
            context.Request.Method = "GET";
            context.Request.Scheme = LibOwin.Infrastructure.Constants.Https;

            var pipeline = new MiddlewareAsLambda().Impl(_noOperation);
            pipeline(context.Environment);

            context.Response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void Impl_NonTestPath_200()
        {
            var context = new OwinContext();
            context.Request.Path = new PathString("/anyOtherPath");
            context.Request.Method = "GET";
            context.Request.Scheme = LibOwin.Infrastructure.Constants.Https;

            var pipeline = new MiddlewareAsLambda().Impl(_noOperation);
            pipeline(context.Environment);

            context.Response.StatusCode.Should().Be(200);
        }
    }
}
