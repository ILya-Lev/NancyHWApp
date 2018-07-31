using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;

namespace LoyaltyProgramEventConsumer
{
    internal class EventSubscriber
    {
        private readonly string _eventFeedHost;
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private readonly int _chunkSize = 100;
        private int _start = 0;

        public EventSubscriber(string eventFeedHost, ILogger logger)
        {
            _eventFeedHost = eventFeedHost;
            _logger = logger;
            _timer = new Timer(10 * 100) { AutoReset = false };
            _timer.Elapsed += (_, __) => SubscriptionCycleCallback().Wait();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public async Task SubscriptionCycleCallback()
        {
            try
            {
                var response = await ReadEvents();
                if (response.StatusCode == HttpStatusCode.OK)
                    HandleEvents(response.Content);
                else
                    _logger.Error($"Response {response.StatusCode} from {response.RequestMessage}");
                _timer.Start();
            }
            catch (Exception exc)
            {
                _logger.Error(exc, "Cannot send request to SpecialOffer event feed");
            }

        }

        private async Task<HttpResponseMessage> ReadEvents()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_eventFeedHost);
                //todo: important to have the accept header - otherwise 500 is returned each time
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"/events?start={_start}&end={_start + _chunkSize}")
                    .ConfigureAwait(false);
                return response;
            }
        }

        private async void HandleEvents(HttpContent content)
        {
            var events = JsonConvert.DeserializeObject<IEnumerable<SpecialOfferEvent>>
                (await content.ReadAsStringAsync());
            foreach (var offerEvent in events)
            {
                _logger.Info($"Handled event with id {offerEvent.SequenceNumber} and name {offerEvent.Name}");
                _start += Math.Max(_start, offerEvent.SequenceNumber);
            }
        }
    }
}