using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            //Init our created DbContext() class via DI
            services.AddDbContext<DataContext>(options =>
            {
                //Here we indicate to use sqlLite and we pass the connection string dt it will use to connect to our database
                //With our _config dt contains the app.settngs.json and appsettings.Development.json, we call the GetconnectionString method dt looks inside d dev file and retreive a key with the value "DefaultConnection"
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
            //Add CORS support
            services.AddCors();
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

            app.UseCors(builder => builder
                .AllowAnyHeader() // we will be sending auth headers from d frontend so ds tells d api to allow any any
                .AllowAnyMethod() // we will b sending get, put, post requests etc from the frontend so ds method tells ds api to allow it
                .WithOrigins("https://localhost:4200")); //ds tells d cors to allow only origin from our angular base url
            app.UseAuthorization();

            //ds take a look at our controllers to see the available endpoints dt are available
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
