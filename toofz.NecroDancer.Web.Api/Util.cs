using System;

namespace toofz.NecroDancer.Web.Api
{
    static class Util
    {
        public static string GetEnvVar(string variable)
        {
            return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine) ??
                throw new ArgumentNullException(null, $"The environment variable '{variable}' must be set.");
        }
    }
}