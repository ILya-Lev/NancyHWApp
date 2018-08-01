using ShoppingCart.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Model
{
    public class ShoppingCartModel
    {
        private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>();
        public int UserId { get; set; }

        public Product[] Products
        {
            get => _products.Values.ToArray();
            set
            {
                if (value?.Any() == true)
                {
                    foreach (var product in value)
                    {
                        if (_products.ContainsKey(product.Id))
                            _products[product.Id] = product;
                        else
                            _products.Add(product.Id, product);
                    }
                }
            }
        }

        public IEnumerable<ShoppingCartItem> Items { get; set; }

        public void AddProducts(IEnumerable<ShoppingCartItem> products, IEventStore eventStore)
        {
            foreach (var cartItem in products)
            {
                if (!_products.ContainsKey(cartItem.Id))
                {
                    _products.Add(cartItem.Id, null);
                }
                _products[cartItem.Id] = new Product()
                {
                    Id = cartItem.Id,
                    Name = cartItem.ProductName,
                    Price = cartItem.Price.Amount
                };

                eventStore.Raise("ShoppingCartItemAdded", new { UserId, cartItem });
            }
        }

        public void RemoveProducts(int[] productIds, IEventStore eventStore)
        {
            foreach (var productId in productIds)
            {
                if (_products.ContainsKey(productId))
                {
                    _products.Remove(productId);
                    eventStore.Raise("ShoppingCartItemRemoved", new { UserId, productId });
                }
            }
        }
    }
}