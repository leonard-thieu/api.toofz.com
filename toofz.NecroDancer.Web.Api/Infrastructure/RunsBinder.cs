using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class RunsBinder : LeaderboardCategoryBaseBinder
    {
        public RunsBinder(Categories leaderboardCategories)
        {
            category = leaderboardCategories["runs"];
        }

        readonly Category category;

        protected override LeaderboardCategoryBase GetModel()
        {
            return new Runs(category);
        }
    }
}