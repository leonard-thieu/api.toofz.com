using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace toofz.NecroDancer.Web.Api.Models
{
    public abstract class CommaSeparatedValues<T> : IEnumerable<T>
        where T : IEquatable<T>
    {
        private readonly HashSet<T> items = new HashSet<T>();

        public void Add(string item)
        {
            items.Add(Convert(item));
        }

        protected abstract T Convert(string item);

        public void AddDefaults()
        {
            var defaults = GetDefaults();
            foreach (var item in defaults)
            {
                items.Add(item);
            }
        }

        protected virtual IEnumerable<T> GetDefaults() => Enumerable.Empty<T>();

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
    }
}