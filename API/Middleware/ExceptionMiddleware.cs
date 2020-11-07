using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;// RequestDelegate : - This is what is coming up next in our http request pipeline (A function dt can manage http requests)..

            _logger = logger; // to log the error
            _environment = environment; // to check d current environment we are running on
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // we process d http request
                await _next(context);
            }
            catch (Exception e)
            {
                // we catch an error and log it and d message to d console
                _logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, e.Message, e.StackTrace)
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");
                // ds ensures our response goes back as json formatted response in camelCase
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                // we convert the http response to a json string using Serialize() passing in the options to use in ds conversion
                var json = JsonSerializer.Serialize(response, options);
                // we write the response (line 40) to the request body
                await context.Response.WriteAsync(json);
            }
        }
    }
}
