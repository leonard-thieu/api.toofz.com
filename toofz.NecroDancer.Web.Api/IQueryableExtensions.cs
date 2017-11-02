using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api
{
    internal static class IQueryableExtensions
    {
        // Modified from http://www.itorian.com/2015/12/sorting-in-webapi-generic-way-to-apply.html
        public static IQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            IDictionary<string, string> keySelectorMap,
            IEnumerable<string> sort)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelectorMap == null)
                throw new ArgumentNullException(nameof(keySelectorMap));

            if (sort == null || !sort.Any()) { return source; }

            var sortExpressions = new List<string>();
            foreach (var token in sort)
            {
                var sortOption = token;
                var isDescending = false;
                if (sortOption.StartsWith("-"))
                {
                    sortOption = token.Remove(0, 1);
                    isDescending = true;
                }

                // Invalid options should already be caught by the model binder.
                var sortExpression = keySelectorMap[sortOption];
                if (isDescending)
                {
                    sortExpression += " descending";
                }
                sortExpressions.Add(sortExpression);
            }

            var ordering = string.Join(", ", sortExpressions);

            return source.OrderBy(ordering);
        }

        public static IQueryable<T> Page<T>(
            this IQueryable<T> source,
            IPagination pagination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pagination == null)
                throw new ArgumentNullException(nameof(pagination));

            // Using properties of a non-mapped object prevents a query from being cached.
            var offset = pagination.Offset;
            var limit = pagination.Limit;

            // Query plans are not parameterized when using the constant overloads for Skip/Take.
            // Using lambda overloads for Skip/Take allows reusing the same query plan for different values.
            return source
                .Skip(() => offset)
                .Take(() => limit);
        }
    }
}