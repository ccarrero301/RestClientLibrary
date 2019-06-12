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
            var postToPut = new BlogPost
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToPutAsJson = JsonConvert.SerializeObject(postToPut);

            var requestInfo = new RestClientRequest(BaseUri, "posts/1", bodyAsJson: postToPutAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.PUT, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(restClientResponse.Result.Id == postToPut.Id);
            Assert.IsTrue(restClientResponse.Result.Title == postToPut.Title);
            Assert.IsTrue(restClientResponse.Result.Body == postToPut.Body);
            Assert.IsTrue(restClientResponse.Result.UserId == postToPut.UserId);
        }

        [Test]
        public void PutDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<RestClientException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.PUT, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}