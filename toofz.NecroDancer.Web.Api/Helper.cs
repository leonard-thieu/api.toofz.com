using System;
using System.Configuration;
using System.Data.Entity.Infrastructure;

namespace toofz.NecroDancer.Web.Api
{
    internal static class Helper
    {
        public static string GetDatabaseConnectionString(string name)
        {
            var connectionString = GetConnectionString(name);
            if (connectionString != null) { return connectionString; }

            var connectionFactory = new LocalDbConnectionFactory("mssqllocaldb");
            using (var connection = connectionFactory.CreateConnection("NecroDancer"))
            {
                return connection.ConnectionString;
            }
        }

        private static string GetConnectionString(string baseName)
        {
            return Environment.GetEnvironmentVariable($"{baseName}ConnectionString", EnvironmentVariableTarget.Machine) ??
                ConfigurationManager.ConnectionStrings[baseName]?.ConnectionString;
        }
    }
}