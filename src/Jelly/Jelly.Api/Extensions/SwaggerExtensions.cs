using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Jelly.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {

                options.CustomSchemaIds(type => type.FullName);

                options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jelly.Api.xml"));
                options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jelly.Resources.xml"));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Jelly.Api",
                    Version = "v1",
                    Description = "这是描述信息",
                    TermsOfService = new Uri("http://www.525600.xyz"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Jelly",
                        Email = "1772829123@qq.com",
                        Url = new Uri("http://www.525600.xyz")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "许可证名字",
                        Url = new Uri("http://www.525600.xyz")
                    }
                });
                //接入IdentityServer
                #region 使用 接入IdentityServer
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows()
                    {
                        #region 方式一 对应 Jelly.IdentityServer InMemoryConfiguration.cs 中的方式一
                        Password = new OpenApiOAuthFlow
                        {

                            AuthorizationUrl = new Uri("http://localhost:8000/connect/authorize"),
                            TokenUrl = new Uri("http://localhost:8000/connect/token"),
                            Scopes = new Dictionary<string, string>
                                {
                                    {"scope1", "Jelly.Api - full access"}
                                }
                        },
                        #endregion
                        #region 方式二 对应 Jelly.IdentityServer InMemoryConfiguration.cs 中的方式二
                        Implicit = new OpenApiOAuthFlow
                        {

                            AuthorizationUrl = new Uri("http://localhost:8000/connect/authorize"),
                            TokenUrl = new Uri("http://localhost:8000/connect/token"),
                            Scopes = new Dictionary<string, string>
                        {
                            {"scope1", "Jelly.Api - full access"}
                        }
                        }
                        #endregion
                    }
                });
                #endregion
                #region 使用Jwt
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                #endregion

                options.OperationFilter<AuthResponsesOperationFilter>();
                options.CustomSchemaIds(type => type.FullName);
            });
            return services;
        }
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
                {
                    //相对路径加载swagger文档 ? 在Ocelot网关中统一配置Swagger https://www.cnblogs.com/focus-lei/p/9047410.html
                    //c.RouteTemplate = "swagger/{documentName}/swagger.json";
                })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jelly.Api");
                    c.RoutePrefix = string.Empty;
                });
            return app;
        }
    }
}