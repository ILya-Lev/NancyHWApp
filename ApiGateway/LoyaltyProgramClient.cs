using LoyaltyProgram.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway
{
    public class LoyaltyProgramClient
    {
        private static readonly string _loyaltyServiceHost = "http://localhost:49745";

        public async Task<LoyaltyProgramUser> RegisterUser(LoyaltyProgramUser newUser)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_loyaltyServiceHost);

                var response = await client.PostAsync("/users/", newUser, new JsonMediaTypeFormatter());

                ThrowOnTransientFailure(response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser>
                    (await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<LoyaltyProgramUser> UpdateUser(LoyaltyProgramUser user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_loyaltyServiceHost);

                var response = await client.PutAsync($"/users/{user.Id}",
                    new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

                ThrowOnTransientFailure(response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser>
                    (await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<LoyaltyProgramUser> QueryUser(int userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_loyaltyServiceHost);

                var response = await client.GetAsync($"/users/{userId}");

                ThrowOnTransientFailure(response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser>
                    (await response.Content.ReadAsStringAsync());
            }
        }

        private void ThrowOnTransientFailure(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Cannot process request {response.RequestMessage}.");
        }
    }
}
