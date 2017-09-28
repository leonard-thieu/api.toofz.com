using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public abstract class LeaderboardCategoryBaseBinder : CommaSeparatedValuesBinder
    {
        public override sealed bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var model = GetModel();
            var success = base.BindModel(actionContext, bindingContext);
            if (success)
            {
                var values = (IEnumerable<string>)bindingContext.Model;
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
            // Parameter not supplied, add all values
            else
            {
                model.AddAll();
                bindingContext.Model = model;

                return true;
            }
        }

        protected abstract LeaderboardCategoryBase GetModel();
    }
}