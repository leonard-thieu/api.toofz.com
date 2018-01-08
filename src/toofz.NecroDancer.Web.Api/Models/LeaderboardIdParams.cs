using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(LeaderboardIdParamsBinder))]
    public sealed class LeaderboardIdParams : CommaSeparatedValues<int>
    {
        protected override int Convert(string item) => int.Parse(item);
    }
}