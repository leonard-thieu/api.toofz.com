using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public abstract class CommaSeparatedValuesBinder : IModelBinder
    {
        public virtual bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var model = GetModel();
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            // Parameter not supplied, add default values
            if (result == null)
            {
                model.AddDefaults();
                bindingContext.Model = model;

                return true;
            }
            else
            {
                var values = result.ConvertTo<string>().Split(',');
                foreach (var value in values)
                {
                    try
                    {
                        model.Add(value);
                    }
                    catch (ArgumentException ex)
                    {
                        bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
                    }
                }

                if (bindingContext.ModelState.IsValid)
                {
                    bindingContext.Model = model;

                    return true;
                }
                else
                {
                    bindingContext.Model = null;

                    return false;
                }
            }
        }

        protected abstract CommaSeparatedValues GetModel();
    }
}