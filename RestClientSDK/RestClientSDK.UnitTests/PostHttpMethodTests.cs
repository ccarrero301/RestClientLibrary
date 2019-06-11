using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestClientSDK.Entities;

namespace RestClientSDK.UnitTests
{
    [TestFixture]
    internal sealed class PostHttpMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task CreateBlogPost()
        {
            var postToCreate = new Post
            {
                Body = "A simple post",
                Title = "A simple post title",
                UserId = 1
            };

            var postToCreateAsJson = JsonConvert.SerializeObject(postToCreate);

            var requestInfo = new RestClientRequest(BaseUri, "posts", bodyAsJson: postToCreateAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<Post>(HttpMethod.Post, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.Created);
            Assert.IsTrue(restClientResponse.Result.Body == postToCreate.Body);
            Assert.IsTrue(restClientResponse.Result.Title == postToCreate.Title);
            Assert.IsTrue(restClientResponse.Result.UserId == postToCreate.UserId);
            Assert.IsTrue(restClientResponse.Result.Id != default);
        }

        [Test]
        public void PostDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<RestClientException>(async () =>
                await RestClient
                    .ExecuteWithExponentialRetryAsync<bool>(HttpMethod.Post, false, 1, 1, HttpStatusCodesWorthRetrying,
                        requestInfo).ConfigureAwait(false));
        }
    }
}