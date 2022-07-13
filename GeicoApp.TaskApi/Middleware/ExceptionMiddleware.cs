using GeicoApp.TaskApi.Exceptions;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System.Net;
using System.Text.Json;
namespace GeicoApp.TaskApi.Middleware
{
    public class PagedList
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PagedList> _logger;
        private readonly IHostEnvironment _env;
        public PagedList(RequestDelegate next, ILogger<PagedList> logger,
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
