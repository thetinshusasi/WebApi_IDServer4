using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_IDServer4.Console_Client
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            #region Resource owner Flow
            Console.WriteLine("Resource owner Flow");
            var discoRo = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discoRo.IsError)
            {
                Console.WriteLine(discoRo.Error);
                return;
            }

            //Grab a bearer token
            var tokenClientRo = new TokenClient(discoRo.TokenEndpoint, "client", "secret");
            var tokenResponseRo = await tokenClientRo.RequestResourceOwnerPasswordAsync("tinshu",
                "tinshu", "MyDBCustomerApi");
            if (tokenResponseRo.IsError)
            {
                Console.WriteLine(tokenResponseRo.Error);
                return;
            }

            Console.WriteLine(tokenResponseRo.Json);
            Console.WriteLine("\n\n");


            #endregion Resource owner Flow


            #region Client Credential Flow

            Console.WriteLine("Client Credential Flow");

            //discover all the endpoints using metadata of identity server
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //Grab a bearer token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("MyDBCustomerApi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            //Consume our Customer API
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                        new { Id = 10, FirstName = "Manish", LastName = "Narayan" }),
                        Encoding.UTF8, "application/json");

            var createCustomerResponse = await client.PostAsync("https://localhost:5001/api/customers"
                                                            , customerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }

            var getCustomerResponse = await client.GetAsync("https://localhost:5001/api/customers");
            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            #endregion Client Credential Flow

            Console.Read();



        }

    }
}
