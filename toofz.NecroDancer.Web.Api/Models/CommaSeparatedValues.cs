using System.Collections;
using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    public abstract class CommaSeparatedValues : IEnumerable<string>
    {
        readonly HashSet<string> items = new HashSet<string>();

        public virtual void Add(string item)
        {
            items.Add(item);
        }

        public void AddDefaults()
        {
            var defaults = GetDefaults();
            foreach (var item in defaults)
            {
                Add(item);
            }
        }

        protected abstract IEnumerable<string> GetDefaults();

        public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
    }
}