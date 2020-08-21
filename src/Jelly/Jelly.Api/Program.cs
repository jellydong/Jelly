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
            #region 配置 Serilog
            Log.Logger = new LoggerConfiguration()
                   //最小的输出级别
                   .MinimumLevel.Information()
                   // 配置日志输出到控制台
                   .WriteTo.Console()
                   // 配置日志输出到文件，文件输出到当前项目的 logs 目录下
                   // 日记的生成周期为每天
                   .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                   // 配置日志输出到MySQL
                   .WriteTo.MySQL("server=127.0.0.1;uid=root;pwd=123456;database=jelly;")
                   // 创建 logger
                   .CreateLogger();

            #endregion
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    //程序一开始的时候初始化数据库的默认数据
                    var myContext = services.GetRequiredService<JellyContext>();
                    JellyContextSeed.SeedAsync(myContext, loggerFactory).Wait();
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "Error occured seeding the Database.初始化数据失败");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                //改用Autofac来实现依赖注入
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 })
                 // 将 Serilog 设置为日志记录提供程序
                 .UseSerilog();
        }
    }
}
