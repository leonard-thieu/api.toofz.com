using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer daily leaderboards.
    /// </summary>
    [DataContract]
    public sealed class DailyLeaderboardsEnvelope
    {
        /// <summary>
        /// Total number of daily leaderboards.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of daily leaderboards.
        /// </summary>
        [DataMember(Name = "leaderboard")]
        public IEnumerable<DailyLeaderboardDTO> Leaderboards { get; set; }
    }
}