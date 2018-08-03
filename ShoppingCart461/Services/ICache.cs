using System;

namespace ShoppingCart.Services
{
    public interface ICache
    {
        void Add(string key, object value, TimeSpan timeToLive);
        object Get(string key);

    }
}