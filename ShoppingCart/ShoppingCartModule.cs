using Nancy;
using Nancy.ModelBinding;
using ShoppingCart.Model;
using ShoppingCart.Services;

namespace ShoppingCart
{
    public class ShoppingCartModule : NancyModule
    {
        public ShoppingCartModule(IShoppingCartStore shoppingCartStore,
                                   IProductCatalogClient productCatalogClient,
                                   IEventStore eventStore)
            : base("/shoppingcart")
        {
            Get("/{userid:int}", parameters => GetShoppingCart(shoppingCartStore, parameters));

            Post("/{userid:int}", async (parameters, _) =>
            {
                var productIds = this.Bind<int[]>();
                var shoppingCart = GetShoppingCart(shoppingCartStore, parameters);

                var products = await productCatalogClient.GetShoppingCartItems(productIds).ConfigureAwait(false);
                shoppingCart.AddProducts(products, eventStore);

                shoppingCartStore.Save(shoppingCart);
                return shoppingCart;
            });

            Delete("/{userid:int}", parameters =>
            {
                var productIds = this.Bind<int[]>(new BindingConfig() { BodyOnly = true });
                var shoppingCart = GetShoppingCart(shoppingCartStore, parameters);

                shoppingCart.RemoveProducts(productIds, eventStore);

                shoppingCartStore.Save(shoppingCart);
                return shoppingCart;
            });
        }

        private static ShoppingCartModel GetShoppingCart(IShoppingCartStore shoppingCartStore, dynamic parameters)
        {
            var userId = (int)parameters.userId;
            return shoppingCartStore.Get(userId);
        }
    }
}
