using SpecialOffers.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecialOffers.Services
{
    class InMemoryEventStore : IEventStore
    {
        private List<Event> _storage = new List<Event>()
        {
            new Event(1, DateTime.Now.AddHours(-1), "Created", new
            {
                OfferID=10,
                Offer = new{ ProductCatalogId=1, ProductName="Tyre", Description="For high speed and pressure"}
            }),
            new Event(2, DateTime.Now, "Updated", new
            {
                OfferID=12,
                Offer = new{ ProductCatalogId=23, ProductName="Engine Oil", Description="Protects your engine"}
            }),
        };

        public IEnumerable<Event> GetEvents(int minEventId, int maxEventId)
            => _storage.Where(e => e.SequenceNumber >= minEventId && e.SequenceNumber <= maxEventId);
    }
}