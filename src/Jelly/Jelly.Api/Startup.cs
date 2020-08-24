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
            services.AddSwaggerGen(options =>
            {

                options.CustomSchemaIds(type => type.FullName);

                options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jelly.Api.xml"));
                options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jelly.Resources.xml"));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Jelly.Api",
                    Version = "v1",
                    Description = "����������Ϣ",
                    TermsOfService = new Uri("http://www.525600.xyz"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Jelly",
                        Email = "1772829123@qq.com",
                        Url = new Uri("http://www.525600.xyz")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "���֤����",
                        Url = new Uri("http://www.525600.xyz")
                    }
                });
                //����IdentityServer
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows()
                    { 
                        #region ��ʽһ ��Ӧ Jelly.IdentityServer InMemoryConfiguration.cs �еķ�ʽһ
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
                        #region ��ʽ�� ��Ӧ Jelly.IdentityServer InMemoryConfiguration.cs �еķ�ʽ��
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
            app.UseSwagger(c =>
                {
                    //���·������swagger�ĵ� ? ��Ocelot������ͳһ����Swagger https://www.cnblogs.com/focus-lei/p/9047410.html
                    //c.RouteTemplate = "swagger/{documentName}/swagger.json";
                })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jelly.Api");
                    c.RoutePrefix = string.Empty;
                });

            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
