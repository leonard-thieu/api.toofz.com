using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using toofz.NecroDancer.Web.Api.ErrorHandler;

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}