using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using RestClientSDK.Entities;
using RestSharp;

namespace RestClientSDK.Utils
{
    internal static class RetryPolicy
    {
        public static Task<IRestResponse<TResult>> ExecuteWithRetryPolicyAsync<TResult>(int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying, Func<Task<IRestResponse<TResult>>> action)
        {
            var retryPolicy = DefineRetryPolicy<TResult>(maxRetryAttempts, retryFactor, httpStatusCodesWorthRetrying);

            return retryPolicy.ExecuteAsync(action);
        }

        private static AsyncRetryPolicy<IRestResponse<TResult>> DefineRetryPolicy<TResult>(int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying) =>
            Policy
                .Handle<RestClientException>()
                .Or<Exception>()
                .OrResult<IRestResponse<TResult>>(restSharpResponse =>
                    httpStatusCodesWorthRetrying.Contains(restSharpResponse.StatusCode))
                .WaitAndRetryAsync(
                    maxRetryAttempts,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryFactor, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => { });
    }
}