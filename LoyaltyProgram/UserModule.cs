using LoyaltyProgram.Model;
using LoyaltyProgram.Services;
using Nancy;
using Nancy.ModelBinding;

namespace LoyaltyProgram
{
    public class UserModule : NancyModule
    {
        //todo: return 404 for not found users on get/update

        public UserModule(ILoyaltyProgramStore loyaltyProgramStore)
            : base("/users")
        {
            Get("/{id:int}", parameters =>
            {
                var userId = (int)parameters.Id;
                return GetUserFromStorage(loyaltyProgramStore, userId);
            });

            Post("/", _ =>
            {
                var user = this.Bind<LoyaltyProgramUser>();
                var userId = loyaltyProgramStore.AddUser(user);
                var storedUser = loyaltyProgramStore.GetUser(userId);

                if (storedUser == null)
                    return HttpStatusCode.NotFound;
                return GenerateCreatedResponse(storedUser);
            });

            Put("/{id:int}", parameters =>
            {
                var userId = (int)parameters.id;
                var user = this.Bind<LoyaltyProgramUser>();

                loyaltyProgramStore.UpdateUser(userId, user);
                return GetUserFromStorage(loyaltyProgramStore, userId);
            });
        }

        private static dynamic GetUserFromStorage(ILoyaltyProgramStore loyaltyProgramStore, int userId)
        {
            var user = loyaltyProgramStore.GetUser(userId);
            if (user == null)
                return HttpStatusCode.NotFound;
            return user;
        }

        private dynamic GenerateCreatedResponse(LoyaltyProgramUser storedUser)
        {
            return Negotiate
                .WithStatusCode(HttpStatusCode.Created)
                .WithHeader("Location", $@"{Request.Url.SiteBase}/users/{storedUser.Id}")
                .WithModel(storedUser);
        }
    }
}
