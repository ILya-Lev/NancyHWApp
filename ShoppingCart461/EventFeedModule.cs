using Nancy;
using ShoppingCart.Services;

namespace ShoppingCart
{
    public class EventFeedModule : NancyModule
    {
        public EventFeedModule(IEventStore eventStore) : base("/events")
        {
            Get("/", _ =>
            {
                if (!int.TryParse(Request.Query.start.Value, out int first))
                    first = 0;
                if (!int.TryParse(Request.Query.end.Value, out int last))
                    last = int.MaxValue;

                return eventStore.GetEvents(first, last);
            });
        }
    }
}
