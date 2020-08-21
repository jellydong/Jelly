using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Jelly.Models.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Jelly.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region ���� Serilog
            Log.Logger = new LoggerConfiguration()
                   //��С���������
                   .MinimumLevel.Information()
                   // ������־���������̨
                   .WriteTo.Console()
                   // ������־������ļ����ļ��������ǰ��Ŀ�� logs Ŀ¼��
                   // �ռǵ���������Ϊÿ��
                   .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                   // ������־�����MySQL
                   .WriteTo.MySQL("server=127.0.0.1;uid=root;pwd=123456;database=jelly;")
                   // ���� logger
                   .CreateLogger();

            #endregion
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    //����һ��ʼ��ʱ���ʼ�����ݿ��Ĭ������
                    var myContext = services.GetRequiredService<JellyContext>();
                    JellyContextSeed.SeedAsync(myContext, loggerFactory).Wait();
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "Error occured seeding the Database.��ʼ������ʧ��");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                //����Autofac��ʵ������ע��
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 })
                 // �� Serilog ����Ϊ��־��¼�ṩ����
                 .UseSerilog();
        }
    }
}
