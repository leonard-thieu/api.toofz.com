using System.ComponentModel;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(PaginationBinder<ItemsPagination>))]
    public sealed class ItemsPagination : IPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int Offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(10)]
        public int Limit { get; set; } = 10;
    }
}