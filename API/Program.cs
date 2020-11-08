using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        // public static void Main(string[] args)
        public static async Task Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();
          var host =  CreateHostBuilder(args).Build();

          //we use ds to bring in our datacontext
          using var scope = host.Services.CreateScope();
          var services = scope.ServiceProvider;

          //we do not have access to the middleware exception handler that catches all exception from d backend which we created inside the Main() here so we need to use try catch
          try
          {
              var context = services.GetRequiredService<DataContext>();
              // ds provides all with great advantage, henceforth we do not need to run "dotnet ef database update' to update our database, all we need is to rerun the app.. even when we drop the database, all we need to bring it back up is to re-run the app
              await context.Database.MigrateAsync();
              // we call our SeedUsers() method to see d json data
              await Seed.SeedUsers(context);
          }
          catch (Exception ex)
          {
              var logger = services.GetRequiredService<ILogger<Program>>();
              logger.LogError(ex, "An Error Occured During Migration!");
          }

          await host.RunAsync();
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
