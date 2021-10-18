using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace BearStock.Authorization.Managers
{
    public class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource> {
                new ApiResource {
                    Name = "SampleApp.API",
                    DisplayName = "SampleApp API",
                    Scopes = {"SampleApp.API"}
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> {
                new ApiScope("SampleApp.API", "SampleApp.API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client> {
                new Client {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "SampleApp.API" },
                    AllowOfflineAccess = true
                }
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser> {
                new TestUser {SubjectId = "1", Username = "user", Password = "password"},
                new TestUser {SubjectId = "2", Username = "krawat10@gmail.com", Password = "fogmbq9i"}
            };
    }
}