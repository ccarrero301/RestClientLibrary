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
    internal sealed class PutHttpMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task PutBlogPost()
        {
            var postToPut = new Post
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToCreateAsJson = JsonConvert.SerializeObject(postToPut);

            var requestInfo = new RestClientRequest(BaseUri, "posts/1", bodyAsJson: postToCreateAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<Post>(HttpMethod.PUT, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(restClientResponse.Result.Id == postToPut.Id);
            Assert.IsTrue(restClientResponse.Result.Title == postToPut.Title);
            Assert.IsTrue(restClientResponse.Result.Body == postToPut.Body);
            Assert.IsTrue(restClientResponse.Result.UserId == postToPut.UserId);
        }
    }
}