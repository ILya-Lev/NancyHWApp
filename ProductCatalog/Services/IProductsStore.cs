using System.Collections.Generic;
using ProductCatalog.Model;

namespace ProductCatalog.Services
{
    public interface IProductsStore
    {
        IEnumerable<Product> GetProductsByIds(IEnumerable<int> productIds);
    }
}