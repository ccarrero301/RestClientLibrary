using System.Collections.Generic;
using System.Net;

namespace RestClientSDK.Entities
{
    public sealed class RestClientResponse<TResult>
    {
        public RestClientResponse(TResult result, HttpStatusCode statusCode, Dictionary<string, string> headers = null)
        {
            Result = result;
            StatusCode = statusCode;
            Headers = headers;
        }

        public TResult Result { get; }

        public HttpStatusCode StatusCode { get; }

        public Dictionary<string, string> Headers { get; }
    }
}