using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api
{
    /// <summary>
    /// Contains extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    internal static class IQueryableExtensions
    {
        // Modified from http://www.itorian.com/2015/12/sorting-in-webapi-generic-way-to-apply.html
        public static IQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            IDictionary<string, string> keySelectorMap,
            IEnumerable<string> sort)
        {
            if (!sort.Any()) { return source; }

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
            // TODO: Verify if this is still true for EF Core.
            // Using properties of a non-mapped object prevents a query from being cached.
            var offset = pagination.Offset;
            var limit = pagination.Limit;

            return source
                .Skip(offset)
                .Take(limit);
        }
    }
}