using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer items.
    /// </summary>
    [DataContract]
    public sealed class ItemsEnvelope
    {
        /// <summary>
        /// The total number of items in the result set.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of items.
        /// </summary>
        [DataMember(Name = "items")]
        public IEnumerable<ItemDTO> Items { get; set; }
    }
}