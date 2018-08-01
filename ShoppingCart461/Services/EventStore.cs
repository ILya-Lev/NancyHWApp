using EventStore.ClientAPI;
using Newtonsoft.Json;
using ShoppingCart.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class EventStore : IEventStore
    {
        //private const string ConnectionString = @"discover://http://127.0.0.1:2113/";
        private const string ConnectionString = @"ConnectTo=tcp://admin:changeit@localhost:1113; HeartBeatTimeout=500";
        private IEventStoreConnection _connection = EventStoreConnection.Create(ConnectionString);
        private static readonly string StreamName = "ShoppingCart";

        public async Task Raise(string eventName, object content)
        {
            await EstablishConnection();

            var contentJson = JsonConvert.SerializeObject(content);
            var metaDataJson = JsonConvert.SerializeObject(new EventMetadata
            {
                EventName = eventName,
                OccurredAt = DateTime.Now
            });

            var eventData = new EventData(Guid.NewGuid(), "ShoppingCartEvent",
                isJson: true,
                data: Encoding.UTF8.GetBytes(contentJson),
                metadata: Encoding.UTF8.GetBytes(metaDataJson));

            await _connection.AppendToStreamAsync(StreamName, ExpectedVersion.Any, eventData);
        }

        public async Task<IEnumerable<Event>> GetEvents(int first, int last)
        {
            await EstablishConnection();

            var result = await _connection.ReadStreamEventsForwardAsync(StreamName, first, last - first, false)
                                          .ConfigureAwait(false);
            return result.Events.Select(e => new
            {
                Content = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Event.Data)),
                Metadata = JsonConvert.DeserializeObject<EventMetadata>(Encoding.UTF8.GetString(e.Event.Metadata))
            })
                .Select((item, idx) =>
                    new Event(idx + first, item.Metadata.OccurredAt, item.Metadata.EventName, item.Content));
        }

        private async Task EstablishConnection()
        {
            try
            {
                await _connection.ConnectAsync().ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                Debug.Print(exc.Message);
            }
        }

        private class EventMetadata
        {
            public string EventName { get; set; }
            public DateTime OccurredAt { get; set; }
        }

    }
}
