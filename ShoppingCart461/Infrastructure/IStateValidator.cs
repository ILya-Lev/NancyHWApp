using System.Threading.Tasks;

namespace ShoppingCart461.Infrastructure
{
    public interface IStateValidator
    {
        Task<bool> IsValidState();
    }
}