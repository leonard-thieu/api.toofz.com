namespace toofz.NecroDancer.Web.Api
{
    // TODO: Refactor this into settings.
    public static class Constants
    {
        public const string SiteName = "toofz API";
#if DEBUG
        public const string MainServer = "https://localhost:49602";
#else
        public const string MainServer = "http://crypt.toofz.com";
#endif
    }
}