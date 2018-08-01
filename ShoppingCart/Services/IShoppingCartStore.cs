using ShoppingCart.Model;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public interface IShoppingCartStore
    {
        Task<ShoppingCartModel> Get(int userId);
        Task Save(ShoppingCartModel shoppingCart);
    }
}