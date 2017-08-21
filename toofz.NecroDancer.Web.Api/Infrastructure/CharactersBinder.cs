using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class CharactersBinder : LeaderboardCategoryBaseBinder
    {
        public CharactersBinder(Categories leaderboardCategories)
        {
            category = leaderboardCategories["characters"];
        }

        readonly Category category;

        protected override LeaderboardCategoryBase GetModel()
        {
            return new Characters(category);
        }
    }
}