using System;
using NUnit.Framework;
using RestClientSDK.Entities;

namespace RestClientSDK.UnitTests
{
    [TestFixture]
    internal sealed class HeadMethodTests : BaseRestClientTestConfiguration
    {
        [SetUp]
        public void Setup() => SetUpConfiguration();

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