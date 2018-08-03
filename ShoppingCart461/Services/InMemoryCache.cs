using System;
using System.Collections.Generic;

namespace ShoppingCart.Services
{
    class InMemoryCache : ICache
    {
        private static readonly Dictionary<string, Item> _storage = new Dictionary<string, Item>();
        public void Add(string key, object value, TimeSpan timeToLive)
        {
            var item = new Item(timeToLive, value);
            if (!_storage.ContainsKey(key))
            {
                _storage.Add(key, item);
            }
            else
            {
                _storage[key] = item;
            }
        }

        public object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            if (!_storage.ContainsKey(key))
                return null;

            var item = _storage[key];
            if (!item.IsStillAlive)
                return null;

            return item.Value;
        }

        private class Item
        {
            private readonly TimeSpan _timeToLive;
            private readonly DateTime _cachedAt;

            public Item(TimeSpan timeToLive, object value)
            {
                _timeToLive = timeToLive;
                _cachedAt = DateTime.Now;
                Value = value;
            }
            public object Value { get; }
            public bool IsStillAlive => DateTime.Now.Subtract(_cachedAt) < _timeToLive;
        }
    }
}