using LoyaltyProgram.Model;
using LoyaltyProgram.Services;
using Nancy;
using Nancy.ModelBinding;

namespace LoyaltyProgram
{
    public class UserModule : NancyModule
    {
        //todo: return 404 for not found users on get/update
        //todo: return 201 on successful user creation with its direct URL

        public UserModule(ILoyaltyProgramStore loyaltyProgramStore)
            : base("/users")
        {
            Get("/{id:int}", parameters =>
            {
                var userId = (int)parameters.Id;
                return loyaltyProgramStore.GetUser(userId);
            });

            Post("/", _ =>
            {
                var user = this.Bind<LoyaltyProgramUser>();
                var userId = loyaltyProgramStore.AddUser(user);
                return loyaltyProgramStore.GetUser(userId);
            });

            Put("/{id:int}", parameters =>
            {
                var userId = (int)parameters.id;
                var user = this.Bind<LoyaltyProgramUser>();

                loyaltyProgramStore.UpdateUser(userId, user);
                return loyaltyProgramStore.GetUser(userId);
            });
        }
    }
}
