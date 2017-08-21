using System.ComponentModel;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(PaginationBinder<ReplaysPagination>))]
    public sealed class ReplaysPagination : IPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(100)]
        public int limit { get; set; } = 100;
    }
}