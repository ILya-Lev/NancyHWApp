using System.Collections.Generic;
using System.Linq;
using ShoppingCart.Model;

namespace ShoppingCart.Services
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private readonly List<ShoppingCartModel> _storage = new List<ShoppingCartModel>();

        public ShoppingCartStore()
        {
            _storage.Add(new ShoppingCartModel { UserId = 123 });
        }

        public ShoppingCartModel Get(int userId)
        {
            return _storage.FirstOrDefault(cart => cart.UserId == userId);
        }

        public void Save(ShoppingCartModel shoppingCart)
        {
            var existingCart = Get(shoppingCart.UserId);
            if (existingCart != null)
                existingCart.UserId = shoppingCart.UserId;//do copying of all properties
            else
                _storage.Add(shoppingCart);
        }
    }
}