using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(PlayersSortParamsBinder))]
    public sealed class PlayersSortParams : CommaSeparatedValues<string>
    {
        protected override string Convert(string item)
        {
            switch (item)
            {
                case "id":
                case "display_name":
                case "updated_at":
                case "entries":
                case "-id":
                case "-display_name":
                case "-updated_at":
                case "-entries":
                    return item;
                default:
                    throw new ArgumentException($"'{item}' is not a valid property to sort by.");
            }
        }

        protected override IEnumerable<string> GetDefaults()
        {
            return new[] { "-entries", "display_name", "id" };
        }
    }
}