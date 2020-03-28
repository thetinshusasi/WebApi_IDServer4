using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_IDServer4.IDServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("MyDBCustomerApi", "Customers in MyDB")

            };

        }

        public static IEnumerable<Client> GetAllClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId="client",
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "MyDBCustomerApi"
                    }

                }
            };
        }
    }
}
