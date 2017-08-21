using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(RunsBinder))]
    public sealed class Runs : LeaderboardCategoryBase
    {
        public Runs(Category category) : base(category) { }
    }
}