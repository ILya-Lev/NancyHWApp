using Nancy;
using Nancy.Responses.Negotiation;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace LoyaltyProgram.Infrastructure
{
    public class YamlBodySerializer : IResponseProcessor
    {
        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return requestedMediaRange.Subtype.ToString().EndsWith("yaml")
                ? new ProcessorMatch
                {
                    ModelResult = MatchResult.DontCare,                         //todo: why these values?
                    RequestedContentTypeResult = MatchResult.NonExactMatch
                }
                : ProcessorMatch.None;
        }

        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new Response
            {
                ContentType = "application/yaml",
                Contents = stream =>
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var serializer = new Serializer();
                        serializer.Serialize(writer, model);
                    }
                }
            };
        }

        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                yield return new Tuple<string, MediaRange>("yaml", new MediaRange("application/yaml"));
            }
        }
    }
}
