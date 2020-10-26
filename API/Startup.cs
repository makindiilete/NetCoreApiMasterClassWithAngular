using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        // inside ds constructor, our app configuration/settings is initialized i.e. appsettings.Development.json and appsettings.json
        public Startup(IConfiguration config) // modified
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // we moved our long code here to ApplicationServiceExtensions.cs and simply call d returned service here
            services.AddApplicationServices(_config);

            services.AddControllers();

            //Add CORS support
            services.AddCors();

            // we moved our long code here to AddIdentityServices.cs and simply call d returned service here
            services.AddIdentityServices(_config);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ds check if we running on dev and then provides the dev exception page when we encounter errors
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //make our app to be able to use routing, from a page to page
            app.UseRouting();

            //Here we add our cors configuration (cors must come btw UseRouting & UseAuthorization)

            //order : - UseCors, UseAuthentication, UseAuthorization..
            app.UseCors(builder => builder
                .AllowAnyHeader() // we will be sending auth headers from d frontend so ds tells d api to allow any any
                .AllowAnyMethod() // we will b sending get, put, post requests etc from the frontend so ds method tells ds api to allow it
                .WithOrigins("https://localhost:4200")); //ds tells d cors to allow only origin from our angular base url
            // we add our middleware here to use our jwt authentication
            app.UseAuthentication();
            app.UseAuthorization();

            //ds take a look at our controllers to see the available endpoints dt are available
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
