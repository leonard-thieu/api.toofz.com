using System.ComponentModel;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(PaginationBinder<PlayersPagination>))]
    public sealed class PlayersPagination : IPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(500)]
        [DefaultValue(100)]
        public int limit { get; set; } = 100;
    }
}