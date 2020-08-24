using System;
using System.Collections.Generic;
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
            services.AddCustomSwagger(new OpenApiInfo
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
            }, new List<string> { "Jelly.Api.xml" }); 
            #endregion
            services.AddControllers();
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
            //Autofac 新增
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //自定义异常过滤器
            app.UseMyExceptionHandler(loggerFactory);

            app.UseRouting();

            app.UseAuthorization();

            // 启用Swagger中间件
            app.UseCustomSwagger(new OpenApiInfo { Title = "Jelly.Api", Version = "v1" });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
