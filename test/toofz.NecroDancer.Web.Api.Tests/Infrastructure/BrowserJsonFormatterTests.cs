using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using toofz.NecroDancer.Web.Api.Infrastructure;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class BrowserJsonFormatterTests
    {
        public class Constructor
        {
            [DisplayFact(nameof(BrowserJsonFormatter))]
            public void ReturnsBrowserJsonFormatter()
            {
                // Arrange
                var serializerSettings = new JsonSerializerSettings();

                // Act
                var formatter = new BrowserJsonFormatter(serializerSettings);

                // Assert
                Assert.IsAssignableFrom<BrowserJsonFormatter>(formatter);
            }

            [DisplayFact]
            public void SetsSerializerSettings()
            {
                // Arrange
                var serializerSettings = new JsonSerializerSettings();

                // Act
                var formatter = new BrowserJsonFormatter(serializerSettings);

                // Assert
                Assert.Same(serializerSettings, formatter.SerializerSettings);
            }

            [Fact(DisplayName = "Add text/html to supported media types")]
            public void AddsTextHtmlToSupportedMediaTypes()
            {
                // Arrange
                var serializerSettings = new JsonSerializerSettings();

                // Act
                var formatter = new BrowserJsonFormatter(serializerSettings);

                // Assert
                Assert.Contains(new MediaTypeHeaderValue("text/html"), formatter.SupportedMediaTypes);
            }
        }

        public class SetDefaultContentHeadersMethod
        {
            [Fact(DisplayName = "Sets content type on headers to application/json")]
            public void SetsContentTypeOnHeadersToApplicationJson()
            {
                // Arrange
                var serializerSettings = new JsonSerializerSettings();
                var formatter = new BrowserJsonFormatter(serializerSettings);
                var type = typeof(object);
                var content = new StringContent("");
                var headers = content.Headers;
                MediaTypeHeaderValue mediaType = null;

                // Act
                formatter.SetDefaultContentHeaders(type, headers, mediaType);

                // Assert
                Assert.Equal(new MediaTypeHeaderValue("application/json"), headers.ContentType);
            }
        }
    }
}
