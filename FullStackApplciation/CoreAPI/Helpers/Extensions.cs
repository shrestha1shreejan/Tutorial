using Microsoft.AspNetCore.Http;

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
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Header");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
