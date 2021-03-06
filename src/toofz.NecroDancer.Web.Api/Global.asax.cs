﻿using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace toofz.NecroDancer.Web.Api
{
    /// <summary>
    /// The ASP.NET Web API application.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        internal static readonly TelemetryClient TelemetryClient = new TelemetryClient();

        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Performs application configuration.
        /// </summary>
        protected void Application_Start()
        {
            TelemetryConfiguration.Active.InstrumentationKey = GetInstrumentationKey();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;
        }

        private static string GetInstrumentationKey()
        {
            const string InstrumentationKeyName = nameof(TelemetryConfiguration.InstrumentationKey);

            return Environment.GetEnvironmentVariable($"toofzApi{InstrumentationKeyName}", EnvironmentVariableTarget.Machine) ??
                   ConfigurationManager.AppSettings[InstrumentationKeyName] ??
                   "";
        }
    }
}
