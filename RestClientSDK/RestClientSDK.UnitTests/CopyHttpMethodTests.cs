using System;
using NUnit.Framework;
using RestClientSDK.Entities;

namespace RestClientSDK.UnitTests
{
    [TestFixture]
    internal sealed class CopyMethodTests : BaseRestClientTestConfiguration
    {
        [SetUp]
        public void Setup() => SetUpConfiguration();

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