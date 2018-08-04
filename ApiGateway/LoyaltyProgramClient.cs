using Newtonsoft.Json;
using Polly;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway
{
    public class LoyaltyProgramClient : ILoyaltyProgramClient
    {
        private static readonly string _loyaltyServiceHost = "http://localhost:49745";

        private static readonly Policy _retryExponentialPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(5, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));

        //it's not working the way it was intended; check health should be done manually
        //private static readonly Policy _retryHeartBeatPolicy = Policy.Handle<Exception>()
        //    .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)),
        //                          async (exc, ts) => await CheckHealth());

        public async Task<LoyaltyProgramUser> RegisterUser(LoyaltyProgramUser newUser)
        {
            return await _retryExponentialPolicy.ExecuteAsync(() => DoRegisterUser(newUser));
        }


        public async Task<LoyaltyProgramUser> UpdateUser(LoyaltyProgramUser user)
        {
            return await _retryExponentialPolicy.ExecuteAsync(() => DoUpdateUser(user));
        }

        public async Task<LoyaltyProgramUser> QueryUser(int userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_loyaltyServiceHost);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"/users/{userId}");

                ThrowOnTransientFailure(response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser>
                    (await response.Content.ReadAsStringAsync());
            }
        }

        private static async Task CheckHealth()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_loyaltyServiceHost, UriKind.Absolute);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                var response = await client.GetAsync("/heartbeat").ConfigureAwait(false);
                ThrowOnTransientFailure(response);
            }
        }

        private async Task<LoyaltyProgramUser> DoRegisterUser(LoyaltyProgramUser newUser)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_loyaltyServiceHost);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = JsonConvert.SerializeObject(newUser);
                var response = await client.PostAsync("/users/", new StringContent(content, Encoding.UTF8, "application/json"));

                ThrowOnTransientFailure(response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser>
                    (await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<LoyaltyProgramUser> DoUpdateUser(LoyaltyProgramUser user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_loyaltyServiceHost);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PutAsync($"/users/{user.Id}",
                    new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

                ThrowOnTransientFailure(response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser>
                    (await response.Content.ReadAsStringAsync());
            }
        }

        private static void ThrowOnTransientFailure(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Cannot process request {response.RequestMessage}.");
        }
    }
}
