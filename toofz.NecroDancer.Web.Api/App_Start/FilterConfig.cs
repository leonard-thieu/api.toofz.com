using System.Web.Mvc;
using toofz.NecroDancer.Web.Api.ErrorHandler;

namespace toofz.NecroDancer.Web.Api
{
    internal static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}