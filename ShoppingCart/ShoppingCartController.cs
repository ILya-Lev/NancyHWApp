using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Model;
using ShoppingCart.Services;

namespace ShoppingCart
{
    [Route("/shoppingcartctrl")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartStore _shoppingCartStore;
        private readonly IProductCatalogClient _productCatalogClient;
        private readonly IEventStore _eventStore;

        public ShoppingCartController(
            IShoppingCartStore shoppingCartStore,
            IProductCatalogClient productCatalogClient,
            IEventStore eventStore)
        {
            _shoppingCartStore = shoppingCartStore;
            _productCatalogClient = productCatalogClient;
            _eventStore = eventStore;
        }

        [HttpGet("/{userId}")]
        public ShoppingCartModel Get(int userId) => _shoppingCartStore.Get(userId);

        [HttpPost("/{userId}")]
        public async Task<ShoppingCartModel> Post(int userId, [FromBody] int[] productIds)
        {
            var cart = _shoppingCartStore.Get(userId);
            var products = await _productCatalogClient.GetShoppingCartItems(productIds).ConfigureAwait(false);

            cart.AddProducts(products, _eventStore);

            _shoppingCartStore.Save(cart);
            return cart;
        }

        [HttpDelete("/{userId")]
        public ShoppingCartModel Delete(int userId, [FromBody] int[] productIds)
        {
            var cart = _shoppingCartStore.Get(userId);

            cart.RemoveProducts(productIds, _eventStore);

            _shoppingCartStore.Save(cart);
            return cart;
        }
    }
}