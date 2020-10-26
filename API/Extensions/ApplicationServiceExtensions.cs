using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //here we setup our ITokenService as dependency for this app and also states the 'TokenService' class as the class that implements ds service..
            services.AddScoped<ITokenService, TokenService>();
            //Init our created DbContext() class via DI
            services.AddDbContext<DataContext>(options =>
            {
                //Here we indicate to use sqlLite and we pass the connection string dt it will use to connect to our database
                //With our _config dt contains the app.settngs.json and appsettings.Development.json, we call the GetconnectionString method dt looks inside d dev file and retreive a key with the value "DefaultConnection"
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
