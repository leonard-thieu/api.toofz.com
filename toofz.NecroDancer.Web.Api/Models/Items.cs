using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer items.
    /// </summary>
    public sealed class Items
    {
        /// <summary>
        /// The total number of items in the result set.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of items.
        /// </summary>
        public IEnumerable<Item> items { get; set; }
    }
}