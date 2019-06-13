using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;
using RestClientSDK.UnitTests.Entities;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class GetHttpMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task GetAllBlogPosts()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<IEnumerable<BlogPost>>(HttpMethod.GET, false, 1, 1,
                    HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Any());
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithHeaderParameter()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts/1");

            requestInfo.AddHeader(("TestHeader", "TestHeaderValue"));

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.GET, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Id == 1);
            Assert.IsTrue(requestInfo.HeaderParameters.FirstOrDefault().Key == "TestHeader");
            Assert.IsTrue(requestInfo.HeaderParameters.FirstOrDefault().Value == "TestHeaderValue");
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithNoParameters()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts/1");

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.GET, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Id == 1);
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithQueryParameter()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            requestInfo.AddQueryParameter(("userId", "1"));

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<IEnumerable<BlogPost>>(HttpMethod.GET, false, 1, 1,
                    HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.Result != null);
            Assert.IsTrue(restClientResponse.Result.Any());
            Assert.IsTrue(restClientResponse.Result.Count(blogPost => blogPost.UserId == 1) ==
                          restClientResponse.Result.Count());
            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task GetBlogPostWithUrlSegment()
        {
            var uriSegments = new Dictionary<string, string> {{"id", "1"}};

            var requestInfo = new RestClientRequest(BaseUri, "posts/{id}", uriSegments: uriSegments);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.GET, false, 1, 1, HttpStatusCodesWorthRetrying,
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

            Assert.ThrowsAsync<RestClientException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.GET, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}