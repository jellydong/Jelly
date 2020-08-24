using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Jelly.Core.Swagger
{
    public class SwaggerOperationFilter : IOperationFilter
    { 
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;

            //先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
                //非匿名的方法,链接中添加 AccessToken 值
                if (!isAnonymous)
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Description = "请输入带有Bearer的Token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,//query header body path formData
                        Required = true //是否必选
                    });
                }
            }
        }
    }
}