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

        //Autofac ����
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

            #region ����ע��
            //AddTransient��˲ʱģʽÿ�����󣬶���ȡһ���µ�ʵ������ʹͬһ�������ȡ���Ҳ���ǲ�ͬ��ʵ��

            //AddScoped��ÿ�����󣬶���ȡһ���µ�ʵ����ͬһ�������ȡ��λ�õ���ͬ��ʵ��

            //AddSingleton����ģʽ��ÿ�ζ���ȡͬһ��ʵ�� 
            #endregion

            #region ע��Swagger���� 
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

        //autofac ����
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // ֱ����Autofacע�������Զ���� 
            builder.RegisterModule(new CustomAutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //�����֤
            app.UseAuthentication();

            //Autofac ����
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //�Զ����쳣������
            app.UseMyExceptionHandler(loggerFactory);

            app.UseRouting();

            app.UseAuthorization();

            // ����Swagger�м��
            app.UseCustomSwagger();

            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
