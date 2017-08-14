using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    class MockLeaderboardCategoryBase : LeaderboardCategoryBase
    {
        public MockLeaderboardCategoryBase(Category category) : base(category) { }
    }
}
