using SpecialOffers.Model;
using System.Collections.Generic;

namespace SpecialOffers.Services
{
    public interface IEventStore
    {
        IEnumerable<Event> GetEvents(int minEventId, int maxEventId);
    }
}