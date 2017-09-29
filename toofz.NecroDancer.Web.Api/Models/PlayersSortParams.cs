using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(PlayersSortParamsBinder))]
    public sealed class PlayersSortParams : CommaSeparatedValues
    {
        public override void Add(string item)
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
                    base.Add(item);
                    break;
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