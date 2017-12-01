using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;

namespace toofz.NecroDancer.Web.Api
{
    public sealed class AiExceptionLogger : ExceptionLogger
    {
        public AiExceptionLogger(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        private readonly TelemetryClient telemetryClient;

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                telemetryClient.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}