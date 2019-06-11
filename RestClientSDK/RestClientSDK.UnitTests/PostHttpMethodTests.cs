using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            var postToCreate = new Post
            {
                Body = "A simple post",
                Title = "A simple post title",
                UserId = 1
            };

            var postToCreateAsJson = JsonConvert.SerializeObject(postToCreate);

            var requestInfo = new RestClientRequest(_baseUri, "posts", bodyAsJson: postToCreateAsJson);

            var restClientResponse = await _restClient
                .ExecuteWithRetryAsync<Post>(HttpMethod.Post, false, 1, 1, _httpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(restClientResponse.Result.Body == postToCreate.Body);
            Assert.IsTrue(restClientResponse.Result.Title == postToCreate.Title);
            Assert.IsTrue(restClientResponse.Result.UserId == postToCreate.UserId);
        }
    }
}