using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ProductsBinder : LeaderboardCategoryBaseBinder
    {
        public ProductsBinder(Categories leaderboardCategories)
        {
            category = leaderboardCategories["products"];
        }

        readonly Category category;

        protected override LeaderboardCategoryBase GetModel()
        {
            return new Products(category);
        }
    }
}