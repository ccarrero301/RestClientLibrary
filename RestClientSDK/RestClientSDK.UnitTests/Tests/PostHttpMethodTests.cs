using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;
using RestClientSDK.UnitTests.Entities;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class PostHttpMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task PostBlogPost()
        {
            var postToCreate = new BlogPost
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToPostAsJson = JsonConvert.SerializeObject(postToCreate);

            var requestInfo = new RestClientRequest(BaseUri, "posts", bodyAsJson: postToPostAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.POST, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(restClientResponse.Result.Id == postToCreate.Id);
            Assert.IsTrue(restClientResponse.Result.Title == postToCreate.Title);
            Assert.IsTrue(restClientResponse.Result.Body == postToCreate.Body);
            Assert.IsTrue(restClientResponse.Result.UserId == postToCreate.UserId);
            Assert.IsTrue(restClientResponse.Headers.Any());
        }

        [Test]
        public async Task PostBlogPostWithHeaderParameter()
        {
            var postToCreate = new BlogPost
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToPostAsJson = JsonConvert.SerializeObject(postToCreate);

            var requestInfo = new RestClientRequest(BaseUri, "posts", bodyAsJson: postToPostAsJson);

            requestInfo.AddHeader(("TestHeader", "TestHeaderValue"));

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.POST, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(restClientResponse.Result.Id == postToCreate.Id);
            Assert.IsTrue(restClientResponse.Result.Title == postToCreate.Title);
            Assert.IsTrue(restClientResponse.Result.Body == postToCreate.Body);
            Assert.IsTrue(restClientResponse.Result.UserId == postToCreate.UserId);
            Assert.IsTrue(requestInfo.HeaderParameters.FirstOrDefault().Key == "TestHeader");
            Assert.IsTrue(requestInfo.HeaderParameters.FirstOrDefault().Value == "TestHeaderValue");
            Assert.IsTrue(restClientResponse.Headers.Any());
        }

        [Test]
        public void PostDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            var restClientException = Assert.ThrowsAsync<RestClientException>(() => RestClient
                .ExecuteWithExponentialRetryAsync<bool>(HttpMethod.POST, false, 1,
                    1, HttpStatusCodesWorthRetrying, requestInfo));

            Assert.IsTrue(restClientException.ErrorResponse.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(restClientException.ErrorResponse.ErrorException != null);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(restClientException.ErrorResponse.ErrorMessage));
            Assert.IsTrue(!string.IsNullOrEmpty(restClientException.ErrorResponse.ResponseContent));
        }
    }
}