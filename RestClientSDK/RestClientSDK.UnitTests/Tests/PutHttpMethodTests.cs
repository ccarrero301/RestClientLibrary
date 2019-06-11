using System;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class PutHttpMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public void DeleteNotImplementedError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await RestClient
                    .ExecuteWithExponentialRetryAsync<bool>(HttpMethod.Put, false, 1, 1, HttpStatusCodesWorthRetrying,
                        requestInfo).ConfigureAwait(false));
        }
    }
}