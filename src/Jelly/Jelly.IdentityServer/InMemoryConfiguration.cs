using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Jelly.IdentityServer
{
    public class InMemoryConfiguration
    {
        /// <summary>
        /// Api Scopes
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("scope1","scope1")
            };
        }
        /// <summary>
        /// ApiResources
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "api1",
                    DisplayName = "My Api1",
                    Scopes = { "scope1" }
                }
            };
        }
        /// <summary>
        /// Clients
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> Clients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "scope1" }
                }
            };
        }
        /// <summary>
        /// Users
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mail@qq.com",
                    Password = "password"
                }
            };
        }
    }
}