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
                    Name = "jellyApi",
                    DisplayName = "jellyApi",
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
                #region 方式一 对应 Jelly.Api Startup.cs 中的方式一
		        new Client
                {
                    ClientId = "jellyApi1",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowAccessTokensViaBrowser = true,//是否通过浏览器为此客户端传输访问令牌
                    RequirePkce = false,
                    RequireClientSecret = false,

                    RedirectUris = {"http://localhost:5000/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"http://localhost:5000","http://localhost:5000"},
                    AllowedScopes = { "scope1" },
                }
	            #endregion
                ,
                #region 方式二 对应 Jelly.Api Startup.cs 中的方式二
                new Client
                {
                ClientId = "jellyApi2",
                AllowedGrantTypes = GrantTypes.Implicit, 
                AllowAccessTokensViaBrowser = true,//是否通过浏览器为此客户端传输访问令牌
                RequirePkce = false,
                RequireClientSecret = false,

                RedirectUris = {"http://localhost:5000/oauth2-redirect.html"},
                AllowedCorsOrigins = {"http://localhost:5000","http://localhost:5000"},
                AllowedScopes = { "scope1" },
                } 
                #endregion
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
                    Username = "admin",
                    Password = "admin"
                }
            };
        }
    }
}