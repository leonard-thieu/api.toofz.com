using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api
{
    internal static class FormatterConfiguration
    {
        public static void Configure(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            var settings = formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.Formatting = Formatting.Indented;

            config.Formatters.Add(new BrowserJsonFormatter(settings));
        }
    }
}