using System;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class MergeMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public void MergeNotImplementedError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.MERGE, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}