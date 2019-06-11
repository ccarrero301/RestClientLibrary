using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestClientSDK.Entities;

namespace RestClientSDK.UnitTests
{
    [TestFixture]
    internal sealed class GetHttpMethodTests : BaseRestClientTestConfiguration
    {
        [SetUp]
        public void Setup()
        {
            SetUpConfiguration();
        }

        [Test]
        public async Task GetAllBlogPostsWithHttpGetMethod()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<IEnumerable<Post>>(HttpMethod.Get, false, 1, 1,
                    HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Any());
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithNoParameters()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts/1");

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<Post>(HttpMethod.Get, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Id == 1);
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithUrlSegment()
        {
            var uriSegments = new Dictionary<string, string> {{"id", "1"}};

            var requestInfo = new RestClientRequest(BaseUri, "posts/{id}", uriSegments: uriSegments);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<Post>(HttpMethod.Get, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Id == 1);
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void GetDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts/1");

            Assert.ThrowsAsync<RestClientException>(async () =>
                await RestClient
                    .ExecuteWithExponentialRetryAsync<bool>(HttpMethod.Get, false, 1, 1, HttpStatusCodesWorthRetrying,
                        requestInfo).ConfigureAwait(false));
        }
    }
}