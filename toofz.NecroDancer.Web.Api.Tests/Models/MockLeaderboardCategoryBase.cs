using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    sealed class MockLeaderboardCategoryBase : LeaderboardCategoryBase
    {
        public MockLeaderboardCategoryBase(Category category) : base(category) { }
    }
}
