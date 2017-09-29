using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class PlayersSortParamBinder : CommaSeparatedValuesBinder
    {
        protected override CommaSeparatedValues GetModel() => new PlayersSortParam();
    }
}