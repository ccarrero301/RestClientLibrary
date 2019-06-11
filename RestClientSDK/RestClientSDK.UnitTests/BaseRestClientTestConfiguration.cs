using System.Net;
using RestClientSDK.Contracts;
using RestClientSDK.Implementations;

namespace RestClientSDK.UnitTests
{
    public abstract class BaseRestClientTestConfiguration
    {
        protected string BaseUri;
        protected HttpStatusCode[] HttpStatusCodesWorthRetrying;
        protected IRestClient RestClient;

        protected virtual void SetUpConfiguration()
        {
            RestClient = new RestClient();

            BaseUri = "jsonplaceholder.typicode.com/";

            HttpStatusCodesWorthRetrying = new[]
            {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
        }
    }
}