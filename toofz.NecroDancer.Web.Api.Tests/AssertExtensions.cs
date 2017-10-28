using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.toofz;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests
{
    internal static class AssertExtensions
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
                // If the server returns an HttpError, we can get a more useful stack trace.
                var httpError = JsonConvert.DeserializeObject<HttpError>(content);
                if (httpError != null)
                {
                    throw new HttpErrorException(httpError, response.StatusCode, response.RequestMessage.RequestUri);
                }

                Assert.Fail(string.Join(Environment.NewLine, statusCodeEx?.Message, contentEx?.Message));
            }
        }
    }
}
