using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}