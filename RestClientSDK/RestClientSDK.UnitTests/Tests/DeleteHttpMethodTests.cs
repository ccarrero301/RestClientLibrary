using System;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class DeleteMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public void DeleteNotImplementedError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.DELETE, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}