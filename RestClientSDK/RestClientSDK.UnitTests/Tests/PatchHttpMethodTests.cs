using System;
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
    internal sealed class PatchMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task PatchBlogPost()
        {
            var postToPatch = new BlogPost
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToPatchAsJson = JsonConvert.SerializeObject(postToPatch);

            var requestInfo = new RestClientRequest(BaseUri, "posts/1", bodyAsJson: postToPatchAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.PATCH, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(restClientResponse.Result.Id == postToPatch.Id);
            Assert.IsTrue(restClientResponse.Result.Title == postToPatch.Title);
            Assert.IsTrue(restClientResponse.Result.Body == postToPatch.Body);
            Assert.IsTrue(restClientResponse.Result.UserId == postToPatch.UserId);
        }

        [Test]
        public void PatchDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<RestClientException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.PATCH, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}