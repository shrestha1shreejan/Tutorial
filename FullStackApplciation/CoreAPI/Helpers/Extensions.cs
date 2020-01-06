using Microsoft.AspNetCore.Http;
using ModelsLibrary.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreAPI.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Method for modifying the global http error response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }


        public static void AddPagination(this HttpResponse response, int currentPage, int itemPerPage, int totalItems, int totalPage)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemPerPage, totalItems, totalPage);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
