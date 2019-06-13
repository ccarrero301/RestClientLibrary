using System;
using System.Net;
using System.Threading.Tasks;
using RestClientSDK.Entities;
using RestClientSDK.Utils;
using RestSharp;
using IRestClient = RestClientSDK.Contracts.IRestClient;

namespace RestClientSDK.Implementations
{
    public sealed class RestClient : IRestClient
    {
        /// <exception cref="T:RestClientSDK.Entities.RestClientException">
        ///     If something fails with the call or with the
        ///     serialization.
        /// </exception>
        public async Task<RestClientResponse<TResult>> ExecuteWithExponentialRetryAsync<TResult>(HttpMethod httpMethod,
            bool useHttp,
            int maxRetryAttempts, int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying,
            RestClientRequest requestInfo)
        {
            var (client, request) = Request.GetRequestConfiguration(httpMethod, useHttp, requestInfo);

            var restResponse = await ExecuteRequestWithRetryPolicyAsync<TResult>(httpMethod, maxRetryAttempts,
                retryFactor, httpStatusCodesWorthRetrying, client, request).ConfigureAwait(false);

            if (restResponse.IsSuccessful)
                return new RestClientResponse<TResult>(restResponse.Data, restResponse.StatusCode,
                    Response.GetHeaders(restResponse));

            var resClientErrorResponse = new RestClientErrorResponse(restResponse.StatusCode, restResponse.Content,
                restResponse.ErrorMessage, restResponse.ErrorException);

            throw new RestClientException(resClientErrorResponse);
        }

        /// <exception cref="T:RestClientSDK.Entities.RestClientException">
        ///     If something fails with the call or with the
        ///     serialization.
        /// </exception>
        public async Task<RestClientResponse<string>> ExecuteWithExponentialRetryAsync(HttpMethod httpMethod,
            bool useHttp,
            int maxRetryAttempts, int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying,
            RestClientRequest requestInfo)
        {
            var (client, request) = Request.GetRequestConfiguration(httpMethod, useHttp, requestInfo);

            var restResponse = await ExecuteRequestWithRetryPolicyAsync(httpMethod, maxRetryAttempts,
                retryFactor, httpStatusCodesWorthRetrying, client, request).ConfigureAwait(false);

            if (restResponse.IsSuccessful)
                return new RestClientResponse<string>(string.Empty, restResponse.StatusCode,
                    Response.GetHeaders(restResponse));

            var resClientErrorResponse = new RestClientErrorResponse(restResponse.StatusCode, restResponse.Content,
                restResponse.ErrorMessage, restResponse.ErrorException);

            throw new RestClientException(resClientErrorResponse);
        }

        private static async Task<IRestResponse<TResult>> ExecuteRequestWithRetryPolicyAsync<TResult>(
            HttpMethod httpMethod, int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying, RestSharp.IRestClient restClient,
            IRestRequest restRequest)
        {
            switch (httpMethod)
            {
                case HttpMethod.GET:
                    return await RetryPolicy.ExecuteWithRetryPolicyAsync(maxRetryAttempts, retryFactor,
                            httpStatusCodesWorthRetrying, () => restClient.ExecuteGetTaskAsync<TResult>(restRequest))
                        .ConfigureAwait(false);

                case HttpMethod.POST:
                    return await RetryPolicy.ExecuteWithRetryPolicyAsync(maxRetryAttempts, retryFactor,
                            httpStatusCodesWorthRetrying, () => restClient.ExecutePostTaskAsync<TResult>(restRequest))
                        .ConfigureAwait(false);

                case HttpMethod.PUT:
                case HttpMethod.PATCH:
                case HttpMethod.DELETE:
                    return await RetryPolicy.ExecuteWithRetryPolicyAsync(maxRetryAttempts, retryFactor,
                            httpStatusCodesWorthRetrying, () => restClient.ExecuteTaskAsync<TResult>(restRequest))
                        .ConfigureAwait(false);

                case HttpMethod.HEAD:
                    throw new NotImplementedException(
                        $"This method is not supported. Please use {nameof(IRestClient.ExecuteWithExponentialRetryAsync)} with no return type");

                case HttpMethod.OPTIONS:
                    throw new NotImplementedException(
                        $"This method is not supported. Please use {nameof(IRestClient.ExecuteWithExponentialRetryAsync)} with no return type");

                default:
                    throw new ArgumentOutOfRangeException(nameof(httpMethod), httpMethod, null);
            }
        }

        private static Task<IRestResponse> ExecuteRequestWithRetryPolicyAsync(HttpMethod httpMethod,
            int maxRetryAttempts, int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying,
            RestSharp.IRestClient restClient, IRestRequest restRequest)
        {
            switch (httpMethod)
            {
                case HttpMethod.HEAD:
                case HttpMethod.OPTIONS:
                    return RetryPolicy.ExecuteWithRetryPolicyAsync(maxRetryAttempts, retryFactor,
                        httpStatusCodesWorthRetrying,
                        () => restClient.ExecuteTaskAsync(restRequest));

                default:
                    throw new ArgumentOutOfRangeException(nameof(httpMethod), httpMethod, null);
            }
        }
    }
}