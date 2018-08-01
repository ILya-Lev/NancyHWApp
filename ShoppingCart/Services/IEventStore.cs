using ShoppingCart.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public interface IEventStore
    {
        Task Raise(string eventName, object content);
        Task<IEnumerable<Event>> GetEvents(int first, int last);
    }
}