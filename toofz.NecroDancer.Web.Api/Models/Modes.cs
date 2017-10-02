using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(ModesBinder))]
    public sealed class Modes : LeaderboardCategoryBase
    {
        public Modes(IEnumerable<string> modes) : base(modes) { }
    }
}