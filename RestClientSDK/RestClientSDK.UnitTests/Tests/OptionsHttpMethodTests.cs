using System;
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
    internal sealed class OptionsMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task OptionsBlogPost()
        {
            var postToHead = new BlogPost
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToOptionsAsJson = JsonConvert.SerializeObject(postToHead);

            var requestInfo = new RestClientRequest(BaseUri, "posts/1", bodyAsJson: postToOptionsAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync(HttpMethod.OPTIONS, false, 1, 1, HttpStatusCodesWorthRetrying,
                    requestInfo).ConfigureAwait(false);

            var httpMethodsAllowed = restClientResponse.Headers.FirstOrDefault(entry => entry.Key == "Access-Control-Allow-Methods");

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.NoContent);
            Assert.IsTrue(string.IsNullOrWhiteSpace(restClientResponse.Result));
            Assert.IsTrue(httpMethodsAllowed.Key == "Access-Control-Allow-Methods");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(httpMethodsAllowed.Value));
        }

        [Test]
        public void OptionsDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts/1");

            Assert.ThrowsAsync<NotImplementedException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.OPTIONS, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}