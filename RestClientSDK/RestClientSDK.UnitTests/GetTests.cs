using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestClientSDK.Contracts;
using RestClientSDK.Entities;
using RestClientSDK.Implementations;

namespace RestClientSDK.UnitTests
{
    internal sealed class GetTests
    {
        private string _baseUri;
        private HttpStatusCode[] _httpStatusCodesWorthRetrying;
        private IRestClient _restClient;

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

        [Test]
        public async Task GetBlogPostWithIdOne()
        {
            var requestInfo = new RestClientRequest(_baseUri, "posts/1");

            var post = await _restClient
                .ExecuteWithRetryAsync<Post>(HttpMethod.Get, false, 1, 1, _httpStatusCodesWorthRetrying, requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(post != null);
        }
    }
}