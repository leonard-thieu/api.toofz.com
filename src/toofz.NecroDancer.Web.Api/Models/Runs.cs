using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(RunsBinder))]
    public sealed class Runs : LeaderboardCategoryBase
    {
        public Runs(IEnumerable<string> runs) : base(runs) { }
    }
}