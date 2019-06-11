using System.Net;

namespace RestClientSDK.Entities
{
    public sealed class RestClientResponse<TResult>
    {
        public RestClientResponse(TResult result, HttpStatusCode statusCode)
        {
            Result = result;
            StatusCode = statusCode;
        }

        public TResult Result { get; }

        public HttpStatusCode StatusCode { get; }
    }
}