using System.Net;
using System.Threading.Tasks;
using RestClientSDK.Entities;
using RestClientSDK.Implementations;

namespace RestClientConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            const string baseUri = "wwwqa.brainshark.com/brainshark/brainshark.services.content/api/v1.0/";
            const string resource = "Presentations/Lock/12131213";

            var httpStatusCodesWorthRetrying = new[]
            {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };

            var requestInfo = new RestClientRequest(baseUri, resource);
            var restClient = new RestClient();

            requestInfo.AddHeader(("Authorization", "Bearer 3c0783e7-f0c0-4083-b267-59621f2b4ae9"));

            var result = await restClient.ExecuteWithRetryAsync<bool>(HttpMethod.Post, false, 1, 1, httpStatusCodesWorthRetrying, requestInfo);
        }
    }
}