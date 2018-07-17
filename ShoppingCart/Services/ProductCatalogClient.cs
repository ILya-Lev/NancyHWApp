using ShoppingCart.Model;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Services
{
    public class ProductCatalogClient : IProductCatalogClient
    {
        private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>()
        {
            [1] = new Product { Id = 1, Name = "toothpaste", Price = 10.99m },
            [2] = new Product { Id = 2, Name = "mouthwash", Price = 23.49m },
        };

        public IEnumerable<Product> GetShoppingCartItems(int[] productIds)
        {
            return productIds
                .Select(id => _products.ContainsKey(id) ? _products[id] : null)
                .Where(p => p != null);
        }
    }
}