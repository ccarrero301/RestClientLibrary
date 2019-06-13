﻿using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;
using RestClientSDK.UnitTests.Entities;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class DeleteMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public async Task DeleteBlogPost()
        {
            var postToDelete = new BlogPost
            {
                Id = 1,
                Title = "A simple post title",
                Body = "A simple post",
                UserId = 1
            };

            var postToDeleteAsJson = JsonConvert.SerializeObject(postToDelete);

            var requestInfo = new RestClientRequest(BaseUri, "posts/1", bodyAsJson: postToDeleteAsJson);

            var restClientResponse = await RestClient
                .ExecuteWithExponentialRetryAsync<BlogPost>(HttpMethod.DELETE, false, 1, 1,
                    HttpStatusCodesWorthRetrying,
                    requestInfo)
                .ConfigureAwait(false);

            Assert.IsTrue(restClientResponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(restClientResponse.Result.Id == default);
            Assert.IsTrue(string.IsNullOrWhiteSpace(restClientResponse.Result.Title));
            Assert.IsTrue(string.IsNullOrWhiteSpace(restClientResponse.Result.Body));
            Assert.IsTrue(restClientResponse.Result.UserId == default);
        }

        [Test]
        public void PatchDeserializationError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts/1");

            Assert.ThrowsAsync<RestClientException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.DELETE, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}