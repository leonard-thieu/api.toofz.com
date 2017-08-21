using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(ProductsBinder))]
    public sealed class Products : LeaderboardCategoryBase
    {
        public Products(Category category) : base(category) { }
    }
}