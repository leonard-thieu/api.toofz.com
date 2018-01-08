using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace toofz.NecroDancer.Web.Api
{
    // https://madskristensen.net/post/cache-busting-in-aspnet
    public static class Fingerprint
    {
        public static string Tag(string rootRelativePath)
        {
            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
                var absolute = HostingEnvironment.MapPath("~" + rootRelativePath);

                string tag = null;
                using (var sha256 = SHA256.Create())
                using (var fs = File.OpenRead(absolute))
                {
                    var hashValue = sha256.ComputeHash(fs);
                    tag = HttpServerUtility.UrlTokenEncode(hashValue);
                }
                var index = rootRelativePath.LastIndexOf('/');

                var result = rootRelativePath.Insert(index, "/sha256-" + tag);
                HttpRuntime.Cache.Insert(rootRelativePath, result, new CacheDependency(absolute));
            }

            return HttpRuntime.Cache[rootRelativePath] as string;
        }
    }
}