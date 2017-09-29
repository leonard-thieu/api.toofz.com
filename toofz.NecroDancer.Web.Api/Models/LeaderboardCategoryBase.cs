using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    public abstract class LeaderboardCategoryBase : CommaSeparatedValues
    {
        protected LeaderboardCategoryBase(Category category)
        {
            this.category = category ?? throw new ArgumentNullException(nameof(category));
        }

        readonly Category category;

        public override void Add(string item)
        {
            if (!category.ContainsKey(item))
                throw new ArgumentException($"'{item}' is not a valid value.");

            base.Add(item);
        }

        protected override IEnumerable<string> GetDefaults() => category.Keys;
    }
}