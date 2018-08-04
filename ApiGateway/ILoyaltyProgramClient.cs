using System.Threading.Tasks;

namespace ApiGateway
{
    public interface ILoyaltyProgramClient
    {
        Task<LoyaltyProgramUser> QueryUser(int userId);
        Task<LoyaltyProgramUser> RegisterUser(LoyaltyProgramUser newUser);
        Task<LoyaltyProgramUser> UpdateUser(LoyaltyProgramUser user);
    }
}