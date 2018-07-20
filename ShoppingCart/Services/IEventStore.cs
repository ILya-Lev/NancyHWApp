using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Services
{
    public interface IEventStore
    {
        void Raise(string eventName, object content);
        IEnumerable GetEvents(int first, int last);
    }

    public class EventStore : IEventStore
    {
        private static readonly Dictionary<int, EventData> _storage = new Dictionary<int, EventData>();
        public void Raise(string eventName, object content)
        {
            var id = _storage.Count + 1;
            _storage.Add(id, new EventData()
            {
                EventName = eventName,
                Content = content,
                Offset = DateTime.UtcNow
            });
        }

        public IEnumerable GetEvents(int first, int last)
        {
            return _storage.Skip(first - 1).Take(last - first).Select(p => p.Value);
        }

        private class EventData
        {
            public string EventName { get; set; }
            public object Content { get; set; }
            public DateTime Offset { get; set; }
        }
    }
}