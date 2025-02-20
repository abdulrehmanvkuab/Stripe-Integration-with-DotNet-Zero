//using System.IO;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Logging;

//namespace Arch.Web.Startup
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateWebHostBuilder(args).Build().Run();
//        }

//        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
//        {
//            return new WebHostBuilder()
//                .UseKestrel(opt =>
//                {
//                    opt.AddServerHeader = false;
//                    opt.Limits.MaxRequestLineSize = 16 * 1024;
//                })
//                .UseContentRoot(Directory.GetCurrentDirectory())
//                .ConfigureLogging((context, logging) =>
//                {
//                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
//                })
//                .UseIIS()
//                .UseIISIntegration()
//                .UseStartup<Startup>();
//        }
//    }
//}
using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Arch.Web.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args) // Uses built-in best practices
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    // Load configuration from appsettings.json and environment-specific appsettings
                    config.SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                })
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                    options.Limits.MaxRequestLineSize = 16 * 1024;
                })
                .UseIIS()
                .UseStartup<Startup>();
        }
    }
}
