using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// The zone and level that the entry ends on.
    /// </summary>
    [DataContract]
    public sealed class EndDTO
    {
        /// <summary>
        /// The zone that the entry ends on.
        /// </summary>
        [DataMember(Name = "zone")]
        public int Zone { get; set; }
        /// <summary>
        /// The level that the entry ends on.
        /// </summary>
        [DataMember(Name = "level")]
        public int Level { get; set; }
    }
}