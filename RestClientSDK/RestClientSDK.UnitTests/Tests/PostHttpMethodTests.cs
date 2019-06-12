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
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToCreateAsJson = JsonConvert.SerializeObject(postToCreate);

            var requestInfo = new RestClientRequest(BaseUri, "posts", bodyAsJson: postToCreateAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.POST, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(restClientResponse.Result.Title == postToCreate.Title);
            Assert.IsTrue(restClientResponse.Result.Body == postToCreate.Body);
            Assert.IsTrue(restClientResponse.Result.UserId == postToCreate.UserId);
            Assert.IsTrue(restClientResponse.Result.Id != default);
        }

        [Test]
        public void PostDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<RestClientException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.POST, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}