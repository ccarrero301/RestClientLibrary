using System;
using RestClientSDK.Entities;
using RestSharp;

namespace RestClientSDK.Utils
{
    internal static class Request
    {
        public static (IRestClient, IRestRequest) GetRequestConfiguration(HttpMethod httpMethod,
            bool useHttp,
            RestClientRequest requestInfo)
        {
            Enum.TryParse<Method>(httpMethod.ToString(), out var restSharpHttpMethod);

            var restClient = GetRestClient(useHttp, requestInfo);
            var request = GetRestRequest(restSharpHttpMethod, requestInfo);

            return (restClient, request);
        }

        private static IRestClient GetRestClient(bool useHttp, RestClientRequest requestInfo)
        {
            var transferProtocol = useHttp ? "http://" : "https://";

            var baseUri = $"{transferProtocol}{requestInfo.BaseUri}";

            return new RestClient(baseUri);
        }

        private static IRestRequest GetRestRequest(Method method, RestClientRequest requestInfo, int timeout = 3600000)
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
    }
}