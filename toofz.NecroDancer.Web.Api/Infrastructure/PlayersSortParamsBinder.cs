using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class PlayersSortParamsBinder : CommaSeparatedValuesBinder<string>
    {
        protected override CommaSeparatedValues<string> GetModel() => new PlayersSortParams();
    }
}