using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(CharactersBinder))]
    public sealed class Characters : LeaderboardCategoryBase
    {
        public Characters(Category category) : base(category) { }
    }
}