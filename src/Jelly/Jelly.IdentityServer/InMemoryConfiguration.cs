using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Jelly.IdentityServer
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "角色", new List<string> { JwtClaimTypes.Role }),
                new IdentityResource("locations", "地点", new List<string> { "location" }),
            };

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
                    Scopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations",
                        "scope1",
                    },
                    ApiSecrets = new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    }
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
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    RedirectUris = {"http://localhost:5000/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"http://localhost:5000","http://localhost:5000"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations",
                        "scope1", //授权的Scopes 
                    },
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
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    RedirectUris = {"http://localhost:5000/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"http://localhost:5000","http://localhost:5000"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations",
                        "scope1", //授权的Scopes 
                    },
                    } 
                #endregion
                ,
                new Client()
                {
                    ClientId = "jellyweb",
                    ClientName = " jellyweb ",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,

                    AccessTokenType = AccessTokenType.Reference,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 60 * 2,
                    RedirectUris = new List<string>()
                    {
                        "http://localhost:8080/CallBack",
                        "http://localhost:8080/SilentCallback"
                    },
                    PostLogoutRedirectUris = {
                        "http://localhost:8080/"
                    },
                    AllowedCorsOrigins = {
                        "http://localhost:8080"
                    },

                    AllowedScopes =
                    {

                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations",
                        "scope1", //授权的Scopes 

                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                }
                ,
                new Client()
                {
                    ClientId = "vuejsclient",
                    ClientName = " vuejs ",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,

                    AccessTokenType = AccessTokenType.Reference,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    AccessTokenLifetime = 60 * 2,
                    RedirectUris = new List<string>()
                    {
                        "http://localhost:8080/static/callback.html",
                        "http://localhost:8080/static/silent-renew.html"
                    },
                    PostLogoutRedirectUris = {
                        "http://localhost:8080/index.html"
                    },
                    AllowedCorsOrigins = {
                        "http://localhost:8080"
                    },

                    AllowedScopes =
                    {

                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations",
                        "scope1", //授权的Scopes 

                    }
                },
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
                new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere")
                    }
                }
            };
        }
    }
}