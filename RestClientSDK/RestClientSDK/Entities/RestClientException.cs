using System;

namespace RestClientSDK.Entities
{
    public sealed class RestClientException : Exception
    {
        public RestClientException(RestClientErrorResponse errorResponse) : base(errorResponse.ErrorMessage,
            errorResponse.ErrorException) =>
            ErrorResponse = errorResponse;

        public RestClientException(RestClientErrorResponse errorResponse, Exception innerException) : base(
            errorResponse.ErrorMessage, innerException) => ErrorResponse = errorResponse;

        public RestClientErrorResponse ErrorResponse { get; }
    }
}