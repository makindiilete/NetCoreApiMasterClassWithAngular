using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            /*
             Initializes a new instance of the HostBuilder class with pre-configured defaults. The following defaults are applied to the returned HostBuilder: - set the ContentRootPath to the result of GetCurrentDirectory() - load host IConfiguration from "DOTNET_" prefixed environment variables - load host IConfiguration from supplied command line args - load app IConfiguration from 'appsettings.json' and 'appsettings.[EnvironmentName].json' - load app IConfiguration from User Secrets when EnvironmentName is 'Development' using the entry assembly - load app IConfiguration from environment variables - load app IConfiguration from supplied command line args - configure the ILoggerFactory to log to the console, debug, and event source output - enables scope validation on the dependency injection container when EnvironmentName is 'Development'
             */
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //ds tells d webuilder to use the Startup.cs class
                    webBuilder.UseStartup<Startup>();
                });
    }
}
