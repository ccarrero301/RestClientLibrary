using System;

namespace RestClientSDK.Entities
{
    public sealed class RestClientException : Exception
    {
        public RestClientException(RestClientErrorResponse errorResponse) : base(errorResponse.ErrorMessage,
            errorResponse.ErrorException) =>
            ErrorResponse = errorResponse;

        public RestClientErrorResponse ErrorResponse { get; }
    }
}