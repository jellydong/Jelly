using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Jelly.Core.Swagger
{
    public static class CustomSwagger
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, OpenApiInfo apiInfo, List<string> xmlFiles)
        {
            services.AddSwaggerGen(options =>
            {

                options.CustomSchemaIds(type => type.FullName);

                foreach (var xmlFile in xmlFiles)
                {
                    options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile));
                }
                options.OperationFilter<SwaggerOperationFilter>();

                options.SwaggerDoc(apiInfo.Version, apiInfo);
                options.CustomSchemaIds(type => type.FullName);
            });
            return services;
        }
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, OpenApiInfo apiInfo)
        {
            app.UseSwagger(c =>
                {
                    //相对路径加载swagger文档 ? 在Ocelot网关中统一配置Swagger https://www.cnblogs.com/focus-lei/p/9047410.html
                    //c.RouteTemplate = "swagger/{documentName}/swagger.json";
                })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/" + apiInfo.Version + "/swagger.json", apiInfo.Title);
                    c.RoutePrefix = string.Empty;
                });
            return app;
        }
    }
}