using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer leaderboards.
    /// </summary>
    [DataContract]
    public sealed class LeaderboardsEnvelope
    {
        /// <summary>
        /// Total number of leaderboards.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of leaderboards.
        /// </summary>
        [DataMember(Name = "leaderboards")]
        public IEnumerable<LeaderboardDTO> Leaderboards { get; set; }
    }
}