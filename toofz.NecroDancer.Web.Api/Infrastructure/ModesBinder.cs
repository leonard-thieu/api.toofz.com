using System;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ModesBinder : CommaSeparatedValuesBinder
    {
        public ModesBinder(Categories leaderboardCategories)
        {
            if (leaderboardCategories == null)
                throw new ArgumentNullException(nameof(leaderboardCategories));

            category = leaderboardCategories["modes"];
        }

        readonly Category category;

        protected override CommaSeparatedValues GetModel() => new Modes(category);
    }
}