namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// The zone and level that the entry ends on.
    /// </summary>
    public sealed class End
    {
        /// <summary>
        /// The zone that the entry ends on.
        /// </summary>
        public int zone { get; set; }
        /// <summary>
        /// The level that the entry ends on.
        /// </summary>
        public int level { get; set; }
    }
}