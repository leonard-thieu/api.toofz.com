using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace toofz.NecroDancer.Web.Api
{
    static class IQueryableExtensions
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

                if (keySelectorMap.TryGetValue(sortOption, out string keySelector))
                {
                    var sortExpression = keySelector;
                    if (isDescending)
                    {
                        sortExpression += " descending";
                    }
                    sortExpressions.Add(sortExpression);
                }
                else
                {
                    var properties = string.Join(", ", from p in keySelectorMap.Values
                                                       select $"'{p}'");
                    throw new ArgumentException($"'{sortOption}' is not a valid property to sort by. Valid properties are: {properties}.");
                }
            }

            var ordering = string.Join(", ", sortExpressions);

            return source.OrderBy(ordering);
        }
    }
}