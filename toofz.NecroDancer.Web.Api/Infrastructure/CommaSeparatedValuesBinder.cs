using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public class CommaSeparatedValuesBinder : IModelBinder
    {
        public virtual bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result == null)
            {
                return false;
            }
            else
            {
                var value = result.ConvertTo<string>();
                bindingContext.Model = value.Split(',');

                return true;
            }
        }
    }
}