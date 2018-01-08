using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class PaginationBinder<T> : IModelBinder
        where T : IPagination, new()
    {
        // Always supplies a value. If a parameter is not supplied, the default value on the type is used.
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var model = new T();
            bindingContext.Model = model;
            var modelState = bindingContext.ModelState;

            var offsetResult = bindingContext.ValueProvider.GetValue("offset");
            if (offsetResult != null)
            {
                try
                {
                    model.Offset = (int)offsetResult.ConvertTo(typeof(int));
                }
                catch
                {
                    modelState.AddModelError("offset", "'offset' is not a valid integer.");
                }
            }

            var limitResult = bindingContext.ValueProvider.GetValue("limit");
            if (limitResult != null)
            {
                try
                {
                    model.Limit = (int)limitResult.ConvertTo(typeof(int));
                }
                catch
                {
                    modelState.AddModelError("limit", "'limit' is not a valid integer.");
                }
            }

            // Data Annotations validation
            var validationNode = bindingContext.ValidationNode;
            validationNode.ValidateAllProperties = true;
            validationNode.Validate(actionContext);

            // Strip prefix from Data Annotations validation errors
            var prefix = bindingContext.ModelName + ".";
            foreach (var key in modelState.Keys.ToList())
            {
                if (key.StartsWith(prefix))
                {
                    var noPrefixKey = key.Remove(0, prefix.Length);
                    foreach (var error in modelState[key].Errors)
                    {
                        modelState.AddModelError(noPrefixKey, error.ErrorMessage);
                    }
                    modelState.Remove(key);
                }
            }

            return true;
        }
    }
}