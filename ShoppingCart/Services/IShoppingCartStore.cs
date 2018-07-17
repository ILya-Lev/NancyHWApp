using ShoppingCart.Model;

namespace ShoppingCart.Services
{
    public interface IShoppingCartStore
    {
        ShoppingCartModel Get(int userId);
        void Save(ShoppingCartModel shoppingCart);
    }
}