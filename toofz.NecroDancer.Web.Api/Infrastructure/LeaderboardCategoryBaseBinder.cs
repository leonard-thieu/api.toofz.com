using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public abstract class LeaderboardCategoryBaseBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var model = GetModel();
            bindingContext.Model = model;
            var modelName = bindingContext.ModelName;
            var result = bindingContext.ValueProvider.GetValue(modelName);

            // Parameter not supplied, add all values
            if (result == null)
            {
                model.AddAll();
            }
            else
            {
                var value = Convert<string>(result);
                foreach (var item in value.Split(','))
                {
                    try
                    {
                        model.Add(item);
                    }
                    catch (ArgumentException ex)
                    {
                        bindingContext.ModelState.AddModelError(modelName, ex.Message);
                    }
                }
            }

            return true;
        }

        protected abstract LeaderboardCategoryBase GetModel();

        T Convert<T>(ValueProviderResult result)
        {
            try
            {
                return (T)result.ConvertTo(typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}