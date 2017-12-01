using System;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace toofz.NecroDancer.Web.Api.ErrorHandler
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public AiHandleErrorAttribute(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        private readonly TelemetryClient telemetryClient;

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                // If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    telemetryClient.TrackException(filterContext.Exception);
                }
            }
            base.OnException(filterContext);
        }
    }
}