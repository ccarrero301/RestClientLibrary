using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestClientSDK.Contracts;
using RestClientSDK.Entities;
using RestClientSDK.Implementations;

namespace RestClientSDK.UnitTests
{
    [TestFixture]
    internal sealed class PostHttpMethodTests
    {
        [SetUp]
        public void Setup()
        {
            _restClient = new RestClient();

            _baseUri = "jsonplaceholder.typicode.com/";

            _httpStatusCodesWorthRetrying = new[]
            {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
        }

        private string _baseUri;
        private HttpStatusCode[] _httpStatusCodesWorthRetrying;
        private IRestClient _restClient;

        [Test]
        public async Task CreateBlogPostWithHttpPostMethod()
        {
            var requestInfo = new RestClientRequest(_baseUri, "posts");

            var restClientResponse = await _restClient
                .ExecuteWithRetryAsync<dynamic>(HttpMethod.Post, false, 1, 1, _httpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.Created);
        }
    }
}