using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RestClientSDK.Entities;
using RestSharp;
using IRestClient = RestClientSDK.Contracts.IRestClient;

namespace RestClientSDK.Implementations
{
    public sealed class RestClient : IRestClient
    {
        public async Task<RestClientResponse<TResult>> ExecuteWithExponentialRetryAsync<TResult>(HttpMethod httpMethod,
            bool useHttp,
            int maxRetryAttempts, int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying,
            RestClientRequest requestInfo)
        {
            var (client, request) = GetRequestConfiguration(httpMethod, useHttp, requestInfo);

            var restResponse = await ExecuteRequestWithRetryPolicyAsync<TResult>(httpMethod, maxRetryAttempts,
                retryFactor, httpStatusCodesWorthRetrying, client, request).ConfigureAwait(false);

            HandleResponseErrors(restResponse);

            return HandleDeserialization(restResponse);
        }

        private static void HandleResponseErrors<TResult>(IRestResponse<TResult> restResponse)
        {
            if (restResponse.IsSuccessful)
                return;

            var resClientErrorResponse = new RestClientErrorResponse(restResponse.StatusCode, restResponse.Content,
                restResponse.ErrorMessage, restResponse.ErrorException);

            throw new RestClientException(resClientErrorResponse);
        }

        private static RestClientResponse<TResult> HandleDeserialization<TResult>(IRestResponse<TResult> restResponse)
        {
            try
            {
                var deserializationResult = JsonConvert.DeserializeObject<TResult>(restResponse.Content);
                return new RestClientResponse<TResult>(deserializationResult, restResponse.StatusCode);
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                var restClientErrorResponse = new RestClientErrorResponse(restResponse.StatusCode, restResponse.Content,
                    jsonSerializationException.Message, jsonSerializationException);

                throw new RestClientException(restClientErrorResponse);
            }
        }

        private static (RestSharp.IRestClient, IRestRequest) GetRequestConfiguration(HttpMethod httpMethod,
            bool useHttp,
            RestClientRequest requestInfo)
        {
            Enum.TryParse<Method>(httpMethod.ToString(), out var restSharpHttpMethod);

            var restClient = GetRestClient(useHttp, requestInfo);
            var request = GetRequest(restSharpHttpMethod, requestInfo);

            return (restClient, request);
        }

        private static RestSharp.IRestClient GetRestClient(bool useHttp, RestClientRequest requestInfo)
        {
            var transferProtocol = useHttp ? "http://" : "https://";

            var baseUri = $"{transferProtocol}{requestInfo.BaseUri}";

            return new RestSharp.RestClient(baseUri);
        }

        private static IRestRequest GetRequest(Method method, RestClientRequest requestInfo, int timeout = 3600000)
        {
            var request = new RestRequest(requestInfo.Resource, method);

            AddHeaderParameters(request, requestInfo);
            AddQueryParameters(request, requestInfo);
            AddUriSegments(request, requestInfo);

            if (requestInfo.BodyAsJson != null)
                request.AddParameter("application/json", requestInfo.BodyAsJson, ParameterType.RequestBody);

            request.Timeout = timeout;

            return request;
        }

        private static void AddHeaderParameters(IRestRequest request, RestClientRequest requestInfo)
        {
            if (requestInfo.HeaderParameters == null)
                return;

            foreach (var headerParameter in requestInfo.HeaderParameters)
                request.AddHeader(headerParameter.Key, headerParameter.Value);
        }

        private static void AddQueryParameters(IRestRequest request, RestClientRequest requestInfo)
        {
            if (requestInfo.QueryParameters == null)
                return;

            foreach (var queryParameter in requestInfo.QueryParameters)
                request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
        }

        private static void AddUriSegments(IRestRequest request, RestClientRequest requestInfo)
        {
            if (requestInfo.UriSegments == null)
                return;

            foreach (var uriSegment in requestInfo.UriSegments)
                request.AddUrlSegment(uriSegment.Key, uriSegment.Value);
        }

        private static async Task<IRestResponse<TResult>> ExecuteRequestWithRetryPolicyAsync<TResult>(
            HttpMethod httpMethod, int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying, RestSharp.IRestClient restClient,
            IRestRequest restRequest)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    return await ExecuteWithRetryPolicyAsync(maxRetryAttempts, retryFactor,
                            httpStatusCodesWorthRetrying, () => restClient.ExecuteGetTaskAsync<TResult>(restRequest))
                        .ConfigureAwait(false);
                case HttpMethod.Post:
                    return await ExecuteWithRetryPolicyAsync(maxRetryAttempts, retryFactor,
                            httpStatusCodesWorthRetrying, () => restClient.ExecutePostTaskAsync<TResult>(restRequest))
                        .ConfigureAwait(false);
                case HttpMethod.Put:
                    throw new NotImplementedException("This method is not yet implemented");
                case HttpMethod.Delete:
                    throw new NotImplementedException("This method is not yet implemented");
                case HttpMethod.Head:
                    throw new NotImplementedException("This method is not yet implemented");
                case HttpMethod.Options:
                    throw new NotImplementedException("This method is not yet implemented");
                case HttpMethod.Patch:
                    throw new NotImplementedException("This method is not yet implemented");
                case HttpMethod.Merge:
                    throw new NotImplementedException("This method is not yet implemented");
                case HttpMethod.Copy:
                    throw new NotImplementedException("This method is not yet implemented");
                default:
                    throw new ArgumentOutOfRangeException(nameof(httpMethod), httpMethod, null);
            }
        }

        private static Task<IRestResponse<TResult>> ExecuteWithRetryPolicyAsync<TResult>(int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying, Func<Task<IRestResponse<TResult>>> action)
        {
            var retryPolicy = DefineRetryPolicy<TResult>(maxRetryAttempts, retryFactor, httpStatusCodesWorthRetrying);

            return retryPolicy.ExecuteAsync(action);
        }

        private static AsyncRetryPolicy<IRestResponse<TResult>> DefineRetryPolicy<TResult>(int maxRetryAttempts,
            int retryFactor, HttpStatusCode[] httpStatusCodesWorthRetrying) =>
            Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<TResult>>(restSharpResponse =>
                    httpStatusCodesWorthRetrying.Contains(restSharpResponse.StatusCode))
                .WaitAndRetryAsync(
                    maxRetryAttempts,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryFactor, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => { });
    }
}