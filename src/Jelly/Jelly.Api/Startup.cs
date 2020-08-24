using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using Jelly.Api.Extensions;
using Jelly.Core.Autofac;
using Jelly.Core.AutoMapper;
using Jelly.Core.Swagger;
using Jelly.Models.Database;
using Jelly.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Jelly.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Autofac 新增
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<JellyContext>(options =>
            {
                var connectionString = this.Configuration["ConnectionStrings:MySqlConn"];
                options.UseMySql(connectionString);
            });
            //AutoMapper
            services.AddAutoMapper(MapperRegister.MapType());

            //FluentValidation
            services.AddTransient<IValidator<PostResource>, PostResourceValidator>();

            #region 依赖注入
            //AddTransient：瞬时模式每次请求，都获取一个新的实例。即使同一个请求获取多次也会是不同的实例

            //AddScoped：每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例

            //AddSingleton单例模式：每次都获取同一个实例 
            #endregion

            #region 注册Swagger服务
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

                options.OperationFilter<AuthResponsesOperationFilter>();
                options.CustomSchemaIds(type => type.FullName);
            });
            #endregion

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = "http://localhost:8000";
                    options.Audience = "jellyApi";
                });

        }

        //autofac 新增
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 直接用Autofac注册我们自定义的 
            builder.RegisterModule(new CustomAutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //身份验证
            app.UseAuthentication();

            //Autofac 新增
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //自定义异常过滤器
            app.UseMyExceptionHandler(loggerFactory);

            app.UseRouting();

            app.UseAuthorization();

            // 启用Swagger中间件
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

            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
