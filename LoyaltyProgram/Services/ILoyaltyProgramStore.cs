using LoyaltyProgram.Model;

namespace LoyaltyProgram.Services
{
    public interface ILoyaltyProgramStore
    {
        LoyaltyProgramUser GetUser(int userId);
        int AddUser(LoyaltyProgramUser user);
        void UpdateUser(int userId, LoyaltyProgramUser user);
    }
}