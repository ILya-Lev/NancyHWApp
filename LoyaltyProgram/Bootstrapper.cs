﻿using Nancy;
using Nancy.Bootstrapper;
using System;

namespace LoyaltyProgram
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
            => NancyInternalConfiguration.WithOverrides(config => config.StatusCodeHandlers.Clear());
    }
}