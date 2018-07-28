using Nancy;
using SpecialOffers.Services;

namespace SpecialOffers
{
    public class EventsFeedModule : NancyModule
    {
        public EventsFeedModule(IEventStore eventStore) : base("/events")
        {
            Get("/", _ =>
            {
                if (!int.TryParse(Request.Query.start, out int minEventId))
                    minEventId = 0;
                if (!int.TryParse(Request.Query.end, out int maxEventId))
                    maxEventId = int.MaxValue;

                return eventStore.GetEvents(minEventId, maxEventId);
            });
        }
    }
}
