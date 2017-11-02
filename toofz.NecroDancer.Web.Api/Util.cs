using System;
using System.Configuration;

namespace toofz.NecroDancer.Web.Api
{
    internal static class Util
    {
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString ??
                throw new InvalidOperationException($"A connection string with the name '{name}' could not be found.");
        }
    }
}