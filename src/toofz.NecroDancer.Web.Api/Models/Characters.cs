using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(CharactersBinder))]
    public sealed class Characters : LeaderboardCategoryBase
    {
        public Characters(IEnumerable<string> characters) : base(characters) { }
    }
}