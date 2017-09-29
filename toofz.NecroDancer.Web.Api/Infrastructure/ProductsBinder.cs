using System;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ProductsBinder : CommaSeparatedValuesBinder
    {
        public ProductsBinder(Categories leaderboardCategories)
        {
            if (leaderboardCategories == null)
                throw new ArgumentNullException(nameof(leaderboardCategories));

            category = leaderboardCategories["products"];
        }

        readonly Category category;

        protected override CommaSeparatedValues GetModel() => new Products(category);
    }
}