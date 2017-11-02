﻿using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using FluentValidation.WebApi;
using Microsoft.Owin.Security.OAuth;

namespace toofz.NecroDancer.Web.Api
{
    internal static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new ValidateModelAttribute());

            RegisterCors(config);
            RegisterRoutes(config);

            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());

            FormatterConfiguration.Configure(config);
            FluentValidationModelValidatorProvider.Configure(config);
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
