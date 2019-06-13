using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace RestClientSDK.Utils
{
    internal static class Response
    {
        public static
            Dictionary<string, string> GetHeaders(IRestResponse restResponse) =>
            restResponse.Headers.ToDictionary(parameter => parameter.Name, parameter => parameter.Value.ToString());
    }
}