using System.ComponentModel;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(PaginationBinder<LeaderboardsPagination>))]
    public sealed class LeaderboardsPagination : IPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int Offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(20)]
        public int Limit { get; set; } = 20;
    }
}