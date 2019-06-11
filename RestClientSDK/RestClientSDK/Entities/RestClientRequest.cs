using System.Collections.Generic;

namespace RestClientSDK.Entities
{
    public sealed class RestClientRequest
    {
        public RestClientRequest(string baseUri, string resource, IDictionary<string, string> headerParameters = null,
            IDictionary<string, string> queryParameters = null, IDictionary<string, string> uriSegments = null,
            string bodyAsJson = null)
        {
            BaseUri = baseUri;
            Resource = resource;
            HeaderParameters = headerParameters;
            QueryParameters = queryParameters;
            UriSegments = uriSegments;
            BodyAsJson = bodyAsJson;
        }

        public string BaseUri { get; }

        public string Resource { get; }

        public IDictionary<string, string> HeaderParameters { get; private set; }

        public IDictionary<string, string> QueryParameters { get; private set; }

        public IDictionary<string, string> UriSegments { get; }

        public string BodyAsJson { get; }

        public void AddHeader((string, string) header)
        {
            if (HeaderParameters == null)
                HeaderParameters = new Dictionary<string, string>();

            var (headerKey, headerValue) = header;

            HeaderParameters.Add(headerKey, headerValue);
        }

        public void AddQueryParameter((string, string) queryParameter)
        {
            if (QueryParameters == null)
                QueryParameters = new Dictionary<string, string>();

            var (queryParameterKey, queryParameterValue) = queryParameter;

            QueryParameters.Add(queryParameterKey, queryParameterValue);
        }
    }
}