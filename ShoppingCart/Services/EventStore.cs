using ShoppingCart.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class EventStore : IEventStore
    {
        public Task Raise(string eventName, object content)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetEvents(int first, int last)
        {
            throw new NotImplementedException();
        }
    }
}
