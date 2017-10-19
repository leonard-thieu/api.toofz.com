using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests
{
    static class AssertExtensions
    {
        public static async Task RespondsWithAsync(
            this Assert assert,
            HttpResponseMessage response,
            string expectedContent,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            Exception statusCodeEx = null;
            try
            {
                Assert.AreEqual(expectedStatusCode, response.StatusCode);
            }
            catch (Exception ex)
            {
                statusCodeEx = ex;
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Exception contentEx = null;
            try
            {
                assert.NormalizedAreEqual(expectedContent, content);
            }
            catch (Exception ex)
            {
                contentEx = ex;
            }

            if (contentEx != null || statusCodeEx != null)
            {
                Assert.Fail(string.Join(Environment.NewLine, statusCodeEx?.Message, contentEx?.Message));
            }
        }
    }
}
