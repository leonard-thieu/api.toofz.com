using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class LeaderboardIdParamsBinder : CommaSeparatedValuesBinder<int>
    {
        protected override CommaSeparatedValues<int> GetModel() => new LeaderboardIdParams();
    }
}