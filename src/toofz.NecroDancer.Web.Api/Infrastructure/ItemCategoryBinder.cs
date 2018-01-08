using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ItemCategoryBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// true if model binding is successful; otherwise, false.
        /// </returns>
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            var result = bindingContext.ValueProvider.GetValue(modelName);

            var value = result.ConvertTo<string>();
            switch (value)
            {
                case "armor":
                case "consumable":
                case "feet":
                case "food":
                case "head":
                case "rings":
                case "scrolls":
                case "spells":
                case "torches":
                case "weapons":
                    bindingContext.Model = value;
                    return true;

                default:
                    bindingContext.ModelState.AddModelError(modelName, $"'{value}' is not a valid value.");
                    return false;
            }
        }
    }
}