using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestClientSDK.Contracts;
using RestClientSDK.Entities;
using RestClientSDK.Implementations;

namespace RestClientSDK.UnitTests
{
    [TestFixture]
    internal sealed class GetHttpMethodTests
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
        public async Task GetAllBlogPostsWithHttpGetMethod()
        {
            var requestInfo = new RestClientRequest(_baseUri, "posts");

            var restClientResponse = await _restClient
                .ExecuteWithRetryAsync<IEnumerable<Post>>(HttpMethod.Get, false, 1, 1, _httpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Any());
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithIdOneNoParametersWithHttpGetMethod()
        {
            var requestInfo = new RestClientRequest(_baseUri, "posts/1");

            var restClientResponse = await _restClient
                .ExecuteWithRetryAsync<Post>(HttpMethod.Get, false, 1, 1, _httpStatusCodesWorthRetrying, requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Id == 1);
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithIdOneWithUrlSegmentWithHttpGetMethod()
        {
            var uriSegments = new Dictionary<string, string> {{"id", "1"}};

            var requestInfo = new RestClientRequest(_baseUri, "posts/{id}", uriSegments: uriSegments);

            var restClientResponse = await _restClient
                .ExecuteWithRetryAsync<Post>(HttpMethod.Get, false, 1, 1, _httpStatusCodesWorthRetrying, requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Id == 1);
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogDeserializationErrorWithHttpGetMethod()
        {
            var requestInfo = new RestClientRequest(_baseUri, "posts/1");

            var restClientResponse = await _restClient
                .ExecuteWithRetryAsync<bool>(HttpMethod.Get, false, 1, 1, _httpStatusCodesWorthRetrying, requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result);
        }
    }
}