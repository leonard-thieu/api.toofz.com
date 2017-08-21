using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ModesBinder : LeaderboardCategoryBaseBinder
    {
        public ModesBinder(Categories leaderboardCategories)
        {
            category = leaderboardCategories["modes"];
        }

        readonly Category category;

        protected override LeaderboardCategoryBase GetModel()
        {
            return new Modes(category);
        }
    }
}