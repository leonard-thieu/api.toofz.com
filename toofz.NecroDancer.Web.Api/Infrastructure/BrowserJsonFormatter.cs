using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    internal sealed class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter(JsonSerializerSettings serializerSettings)
        {
            SerializerSettings = serializerSettings;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}