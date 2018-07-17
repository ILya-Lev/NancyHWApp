using System.Collections.Generic;
using ShoppingCart.Model;

namespace ShoppingCart.Services
{
    public interface IProductCatalogClient
    {
        IEnumerable<Product> GetShoppingCartItems(int[] productIds);
    }
}