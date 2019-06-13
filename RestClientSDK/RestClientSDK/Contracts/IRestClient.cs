using System.Net;
using System.Threading.Tasks;
using RestClientSDK.Entities;

namespace RestClientSDK.Contracts
{
    public interface IRestClient
    {
        Task<RestClientResponse<TResult>> ExecuteWithExponentialRetryAsync<TResult>(HttpMethod httpMethod, bool useHttp,
            int maxRetryAttempts, int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying,
            RestClientRequest requestInfo);

        Task<RestClientResponse<string>> ExecuteWithExponentialRetryAsync(HttpMethod httpMethod, bool useHttp,
            int maxRetryAttempts, int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying,
            RestClientRequest requestInfo);
    }
}