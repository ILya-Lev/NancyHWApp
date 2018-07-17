using ShoppingCart.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Model
{
    public class ShoppingCartModel
    {
        private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>();
        public int UserId { get; set; }
        public Product[] Products => _products.Values.ToArray();

        public void AddProducts(IEnumerable<Product> products, IEventStore eventStore)
        {
            foreach (var product in products)
            {
                if (!_products.ContainsKey(product.Id))
                {
                    _products.Add(product.Id, null);
                }
                _products[product.Id] = product;
            }
        }

        public void RemoveProducts(int[] productIds, IEventStore eventStore)
        {
            foreach (var productId in productIds)
            {
                if (_products.ContainsKey(productId))
                    _products.Remove(productId);
            }
        }
    }
}