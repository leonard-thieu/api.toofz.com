using System;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class CharactersBinder : CommaSeparatedValuesBinder
    {
        public CharactersBinder(Categories leaderboardCategories)
        {
            if (leaderboardCategories == null)
                throw new ArgumentNullException(nameof(leaderboardCategories));

            category = leaderboardCategories["characters"];
        }

        readonly Category category;

        protected override CommaSeparatedValues GetModel() => new Characters(category);
    }
}