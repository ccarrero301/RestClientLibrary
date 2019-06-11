using System;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class CopyMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public void CopyNotImplementedError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await RestClient
                    .ExecuteWithExponentialRetryAsync<bool>(HttpMethod.Copy, false, 1, 1, HttpStatusCodesWorthRetrying,
                        requestInfo).ConfigureAwait(false));
        }
    }
}