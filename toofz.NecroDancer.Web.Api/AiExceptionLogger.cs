using System.Diagnostics.CodeAnalysis;
using System.Web.Http.ExceptionHandling;

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    public sealed class AiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                WebApiApplication.TelemetryClient.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}