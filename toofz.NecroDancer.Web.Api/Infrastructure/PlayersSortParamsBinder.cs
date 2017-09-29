using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class PlayersSortParamsBinder : CommaSeparatedValuesBinder
    {
        protected override CommaSeparatedValues GetModel() => new PlayersSortParams();
    }
}