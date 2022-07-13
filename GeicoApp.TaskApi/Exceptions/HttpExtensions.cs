using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System.Text.Json;
using GeicoApp.TaskApi.Helpers;
using GeicoApp.TaskApi.Middleware;

namespace GeicoApp.TaskApi.Exceptions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
            int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
