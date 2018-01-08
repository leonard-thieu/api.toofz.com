using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(ProductsBinder))]
    public sealed class Products : LeaderboardCategoryBase
    {
        public Products(IEnumerable<string> products) : base(products) { }
    }
}