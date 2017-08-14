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
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(10)]
        public int limit { get; set; } = 10;
    }

    [ModelBinder(BinderType = typeof(PaginationBinder<EnemiesPagination>))]
    public sealed class EnemiesPagination : IPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(10)]
        public int limit { get; set; } = 10;
    }

    [ModelBinder(BinderType = typeof(PaginationBinder<LeaderboardsPagination>))]
    public sealed class LeaderboardsPagination : IPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(20)]
        public int limit { get; set; } = 20;
    }

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