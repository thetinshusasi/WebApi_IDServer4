using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_IDServer4.IDServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId="1",
                    Username="tinshu",
                    Password="tinshu"

                },
                 new TestUser()
                {
                      SubjectId="2",
                    Username="james",
                    Password="james"
                }
            };
        }
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
                /// Client Credential
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

                },
                ///Resourcer owner client
                 new Client()
                {
                    ClientId="ro.client",
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "MyDBCustomerApi"
                    }

                },

                 ///Implicit flow

                  new Client()
                {
                    ClientId="impl.client",
                    ClientName ="MVC client",
                    AllowedGrantTypes= GrantTypes.Implicit,
                    RedirectUris =
                      {
                          "http://localhost:5003/signin-oidc"
                      },
                      PostLogoutRedirectUris =
                      {
                       "http://localhost:5003/signout-callback-oidc"

                      },
                    AllowedScopes = new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile

                    }

                },

                   new Client()
                {
                    ClientId="swaggerapiui",
                    ClientName ="Swagger API UI",
                    AllowedGrantTypes= GrantTypes.Implicit,
                    
                    RedirectUris =
                      {
                          "https://localhost:5001/swagger/oauth2-redirect.html"
                      },
                      PostLogoutRedirectUris =
                      {
                       "https://localhost:5001/swagger"

                      },
                    AllowedScopes =
                    {
                        "MyDBCustomerApi"
                    },
                    AllowAccessTokensViaBrowser=true

                }
            };
        }
    }
}
