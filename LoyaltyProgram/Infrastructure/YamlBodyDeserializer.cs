using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using System;
using System.IO;
using YamlDotNet.Serialization;

namespace LoyaltyProgram.Infrastructure
{
    public class YamlBodyDeserializer : IBodyDeserializer
    {
        public bool CanDeserialize(MediaRange mediaRange, BindingContext context)
        {
            return mediaRange.Subtype.ToString().EndsWith("yaml", StringComparison.InvariantCultureIgnoreCase);
        }

        public object Deserialize(MediaRange mediaRange, Stream bodyStream, BindingContext context)
        {
            using (var reader = new StreamReader(bodyStream))
            {
                var deserializer = new Deserializer();
                return deserializer.Deserialize(reader, context.DestinationType);
            }
        }
    }
}
