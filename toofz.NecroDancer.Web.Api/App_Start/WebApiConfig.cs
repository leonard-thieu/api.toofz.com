using System.Web.Http;
using System.Web.Http.Cors;

namespace toofz.NecroDancer.Web.Api
{
    internal static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new ValidateModelAttribute());

            RegisterCors(config);
            RegisterRoutes(config);

            FormatterConfiguration.Configure(config);
        }

        private static void RegisterCors(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*", "Date");
            config.EnableCors(cors);
        }

        private static void RegisterRoutes(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
