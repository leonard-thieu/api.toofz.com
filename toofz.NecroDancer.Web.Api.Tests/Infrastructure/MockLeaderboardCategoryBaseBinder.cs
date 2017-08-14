using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class MockLeaderboardCategoryBaseBinder : LeaderboardCategoryBaseBinder
    {
        public MockLeaderboardCategoryBaseBinder(LeaderboardCategoryBase model)
        {
            this.model = model;
        }

        readonly LeaderboardCategoryBase model;

        protected override LeaderboardCategoryBase GetModel()
        {
            return model;
        }
    }
}
