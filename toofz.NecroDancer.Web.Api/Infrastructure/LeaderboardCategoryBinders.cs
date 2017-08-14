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