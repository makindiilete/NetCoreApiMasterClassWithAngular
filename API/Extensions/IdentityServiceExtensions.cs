using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //We add our jwt authentication with dependency injection
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // ds says d token sud b validated
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                configuration[
                                    "TokenKey"])), // d same key we used inside our TokenService will b used here to validate our token using the 'configuration' field of IConfiguration interface
                    ValidateIssuer = false, // d issuer is our api server
                    ValidateAudience = false, // d audience is our angular app
                });
            return services;
        }
    }
}
