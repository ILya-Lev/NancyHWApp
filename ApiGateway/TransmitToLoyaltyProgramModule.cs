using Nancy;
using Nancy.ModelBinding;

namespace ApiGateway
{
    public class TransmitToLoyaltyProgramModule : NancyModule
    {
        public TransmitToLoyaltyProgramModule(ILoyaltyProgramClient lpClient) : base("/tolp")
        {
            Get("/{id:int}", parameters =>
            {
                var id = (int)parameters.id;
                return lpClient.QueryUser(id);
            });

            Post("/", async _ =>
            {
                var user = this.Bind<LoyaltyProgramUser>();
                return await lpClient.RegisterUser(user).ConfigureAwait(false);
            });

            Put("/", async _ =>
            {
                var user = this.Bind<LoyaltyProgramUser>();
                return await lpClient.UpdateUser(user).ConfigureAwait(false);
            });
        }
    }

    public class LoyaltyProgramUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
