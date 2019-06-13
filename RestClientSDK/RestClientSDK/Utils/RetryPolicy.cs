using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
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

        public static Task<IRestResponse> ExecuteWithRetryPolicyAsync(int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying, Func<Task<IRestResponse>> action)
        {
            var retryPolicy = DefineRetryPolicy(maxRetryAttempts, retryFactor, httpStatusCodesWorthRetrying);

            return retryPolicy.ExecuteAsync(action);
        }

        public static AsyncRetryPolicy<IRestResponse<TResult>> DefineRetryPolicy<TResult>(int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying) =>
            Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<TResult>>(restSharpResponse =>
                    httpStatusCodesWorthRetrying.Contains(restSharpResponse.StatusCode))
                .WaitAndRetryAsync(
                    maxRetryAttempts,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryFactor, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => { });

        public static AsyncRetryPolicy<IRestResponse> DefineRetryPolicy(int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying) =>
            Policy
                .Handle<Exception>()
                .OrResult<IRestResponse>(restSharpResponse =>
                    httpStatusCodesWorthRetrying.Contains(restSharpResponse.StatusCode))
                .WaitAndRetryAsync(
                    maxRetryAttempts,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryFactor, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => { });
    }
}