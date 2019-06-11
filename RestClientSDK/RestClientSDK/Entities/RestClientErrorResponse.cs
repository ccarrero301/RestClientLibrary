using System;
using System.Net;

namespace RestClientSDK.Entities
{
    public sealed class RestClientErrorResponse
    {
        public RestClientErrorResponse(HttpStatusCode statusCode, string responseContent, string errorMessage,
            Exception errorException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
            ErrorMessage = errorMessage;
            ErrorException = errorException;
        }

        public HttpStatusCode StatusCode { get; }

        public string ResponseContent { get; }

        public string ErrorMessage { get; }

        public Exception ErrorException { get; }
    }
}