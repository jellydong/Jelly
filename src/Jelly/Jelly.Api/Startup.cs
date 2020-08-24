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
            services.AddCustomSwagger();
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
            app.UseCustomSwagger();

            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
