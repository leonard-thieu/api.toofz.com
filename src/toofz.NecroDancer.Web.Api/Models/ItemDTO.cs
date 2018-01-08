using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer item.
    /// </summary>
    [DataContract]
    public sealed class ItemDTO
    {
        /// <summary>
        /// The item's name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// The item's display name.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
        /// <summary>
        /// The slot that the item can be equipped in.
        /// </summary>
        [DataMember(Name = "slot")]
        public string Slot { get; set; }
        /// <summary>
        /// The item's unlock cost.
        /// </summary>
        [DataMember(Name = "unlock")]
        public int? Unlock { get; set; }
        /// <summary>
        /// The item's base price.
        /// </summary>
        [DataMember(Name = "cost")]
        public int? Cost { get; set; }
    }
}