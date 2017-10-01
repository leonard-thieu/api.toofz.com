using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(ItemSubcategoryFilterBinder))]
    public sealed class ItemSubcategoryFilter
    {
        public string Category { get; set; }
        public string Subcategory { get; set; }
    }
}