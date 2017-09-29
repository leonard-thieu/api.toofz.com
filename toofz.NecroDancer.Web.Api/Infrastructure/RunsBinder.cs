using System;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class RunsBinder : CommaSeparatedValuesBinder
    {
        public RunsBinder(Categories leaderboardCategories)
        {
            if (leaderboardCategories == null)
                throw new ArgumentNullException(nameof(leaderboardCategories));

            category = leaderboardCategories["runs"];
        }

        readonly Category category;

        protected override CommaSeparatedValues GetModel() => new Runs(category);
    }
}