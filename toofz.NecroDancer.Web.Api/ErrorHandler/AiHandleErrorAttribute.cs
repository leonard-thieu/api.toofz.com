using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace toofz.NecroDancer.Web.Api.ErrorHandler
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                // If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    WebApiApplication.TelemetryClient.TrackException(filterContext.Exception);
                }
            }
            base.OnException(filterContext);
        }
    }
}