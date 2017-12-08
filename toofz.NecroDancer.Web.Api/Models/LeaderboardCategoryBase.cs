using System;
using System.Collections.Generic;
using System.Linq;

namespace toofz.NecroDancer.Web.Api.Models
{
    public abstract class LeaderboardCategoryBase : CommaSeparatedValues<string>
    {
        protected LeaderboardCategoryBase(IEnumerable<string> values)
        {
            // Sanity check
            // An improperly initialized database can provide empty values.
            if (!values.Any())
                throw new ArgumentException($"'{nameof(values)}' does not contain any values.", nameof(values));

            this.values = values.ToList().AsReadOnly();
        }

        private readonly IEnumerable<string> values;

        protected override string Convert(string item)
        {
            if (!values.Contains(item))
                throw new ArgumentException($"'{item}' is not a valid value.");

            return item;
        }

        protected override IEnumerable<string> GetDefaults() => values;
    }
}