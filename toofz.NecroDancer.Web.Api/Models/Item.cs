namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer item.
    /// </summary>
    public sealed class Item
    {
        /// <summary>
        /// The item's name.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The item's display name.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// The slot that the item can be equipped in.
        /// </summary>
        public string slot { get; set; }
        /// <summary>
        /// The item's unlock cost.
        /// </summary>
        public int? unlock { get; set; }
        /// <summary>
        /// The item's base price.
        /// </summary>
        public int? cost { get; set; }
    }
}