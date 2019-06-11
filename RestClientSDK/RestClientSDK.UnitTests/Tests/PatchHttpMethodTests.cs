﻿using System;
using NUnit.Framework;
using RestClientSDK.Entities;
using RestClientSDK.UnitTests.Base;

namespace RestClientSDK.UnitTests.Tests
{
    [TestFixture]
    internal sealed class PatchMethodTests : BaseRestClientTestConfiguration
    {
        [Test]
        public void PatchNotImplementedError()
        {
            var requestInfo = new RestClientRequest(BaseUri, "posts");

            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await RestClient
                    .ExecuteWithExponentialRetryAsync<bool>(HttpMethod.Patch, false, 1, 1, HttpStatusCodesWorthRetrying,
                        requestInfo).ConfigureAwait(false));
        }
    }
}