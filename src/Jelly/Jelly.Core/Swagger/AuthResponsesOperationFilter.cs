using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Jelly.Core.Swagger
{
    public class AuthResponsesOperationFilter : IOperationFilter
    { 
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //获取是否添加登录特性
            // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            //var hasAuthorize =
            //    context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
            //    || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>().Any();
            

            if (authAttributes)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "暂无访问权限" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "禁止访问" });
                operation.Security=new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        [
                            new OpenApiSecurityScheme {Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"}
                            }
                        ] = new[] { "jellyApi" }
                    }
                };
            }
        }
    }
}