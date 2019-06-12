using System;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class OptionsMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public void OptionsNotImplementedError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(() =>
                RestClient.ExecuteWithExponentialRetryAsync<bool>(HttpMethod.OPTIONS, false, 1, 1,
                    HttpStatusCodesWorthRetrying, requestInfo));
        }
    }
}