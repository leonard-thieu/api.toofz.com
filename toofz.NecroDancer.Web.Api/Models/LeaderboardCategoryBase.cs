using System;
using System.Collections;
using System.Collections.Generic;
using toofz.NecroDancer.Leaderboards;

namespace toofz.NecroDancer.Web.Api.Models
{
    public abstract class LeaderboardCategoryBase : IEnumerable<string>
    {
        public LeaderboardCategoryBase(Category category)
        {
            this.category = category;
        }

        readonly Category category;
        readonly HashSet<string> items = new HashSet<string>();

        public void Add(string item)
        {
            if (!category.ContainsKey(item))
            {
                throw new ArgumentException($"'{item}' is not a valid value.");
            }
            items.Add(item);
        }

        public void AddAll()
        {
            foreach (var value in category.Keys)
            {
                items.Add(value);
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<string>)items).GetEnumerator();
        }
    }
}