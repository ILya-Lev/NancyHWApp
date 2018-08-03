using ProductCatalog.Model;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.Services
{
    public class ProductsStore : IProductsStore
    {
        private static readonly Dictionary<int, Product> _storage = new Dictionary<int, Product>()
        {
            [1] = new Product { Id = 1, Name = "Apple", Price = 10m },
            [2] = new Product { Id = 2, Name = "Peak", Price = 11m }
        };

        public IEnumerable<Product> GetProductsByIds(IEnumerable<int> productIds)
        {
            return productIds
                .Where(id => _storage.ContainsKey(id))
                .Select(id => _storage[id]);
        }
    }
}