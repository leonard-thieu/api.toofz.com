using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Models
{
    [ModelBinder(BinderType = typeof(ProductsBinder))]
    public sealed class Products : LeaderboardCategoryBase
    {
        public Products(Category category) : base(category) { }
    }

    [ModelBinder(BinderType = typeof(ModesBinder))]
    public sealed class Modes : LeaderboardCategoryBase
    {
        public Modes(Category category) : base(category) { }
    }

    [ModelBinder(BinderType = typeof(RunsBinder))]
    public sealed class Runs : LeaderboardCategoryBase
    {
        public Runs(Category category) : base(category) { }
    }

    [ModelBinder(BinderType = typeof(CharactersBinder))]
    public sealed class Characters : LeaderboardCategoryBase
    {
        public Characters(Category category) : base(category) { }
    }
}