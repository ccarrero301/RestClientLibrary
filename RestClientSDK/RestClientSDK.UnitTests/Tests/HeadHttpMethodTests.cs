using System;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class HeadMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public void HeadNotImplementedError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await RestClient
                    .ExecuteWithExponentialRetryAsync<bool>(HttpMethod.Head, false, 1, 1, HttpStatusCodesWorthRetrying,
                        requestInfo).ConfigureAwait(false));
        }
    }
}