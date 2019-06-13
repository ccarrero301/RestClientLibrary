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
    internal sealed class HeadMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task HeadBlogPost()
        {
            var postToHead = new BlogPost
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToHeadAsJson = JsonConvert.SerializeObject(postToHead);

            var requestInfo = new RestClientRequest(BaseUri, "posts/1", bodyAsJson: postToHeadAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync(HttpMethod.HEAD, false, 1, 1, HttpStatusCodesWorthRetrying, requestInfo).ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(string.IsNullOrWhiteSpace(restClientResponse.Result));
        }

        [Test]
        public void HeadDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.HEAD, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}