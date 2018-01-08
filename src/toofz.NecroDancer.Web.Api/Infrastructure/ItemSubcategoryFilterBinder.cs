using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ItemSubcategoryFilterBinder : IModelBinder
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
            var modelState = bindingContext.ModelState;

            var categoryResult = bindingContext.ValueProvider.GetValue("category");
            var category = categoryResult.ConvertTo<string>();
            switch (category)
            {
                case "weapons":
                case "chest":
                    var model = new ItemSubcategoryFilter();
                    model.Category = category;
                    var subcategoryResult = bindingContext.ValueProvider.GetValue("subcategory");
                    var subcategory = subcategoryResult.ConvertTo<string>();
                    switch (category)
                    {
                        case "weapons":
                            switch (subcategory)
                            {
                                case "bows":
                                case "broadswords":
                                case "cats":
                                case "crossbows":
                                case "daggers":
                                case "flails":
                                case "longswords":
                                case "rapiers":
                                case "spears":
                                case "whips":
                                    model.Subcategory = subcategory;
                                    bindingContext.Model = model;
                                    return true;

                                default:
                                    modelState.AddModelError("subcategory", $"'{subcategory}' is not a valid subcategory.");
                                    return false;
                            }
                        case "chest":
                            switch (subcategory)
                            {
                                case "red":
                                case "purple":
                                case "black":
                                case "mimic":
                                    model.Subcategory = subcategory;
                                    bindingContext.Model = model;
                                    return true;

                                default:
                                    modelState.AddModelError("subcategory", $"'{subcategory}' is not a valid subcategory.");
                                    return false;
                            }
                        // Unreachable
                        default:
                            return false;
                    }
                default:
                    modelState.AddModelError("category", $"'{category}' is not a valid category.");
                    return false;
            }
        }
    }
}