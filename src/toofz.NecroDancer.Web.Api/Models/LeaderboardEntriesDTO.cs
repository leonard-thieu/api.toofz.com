using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer leaderboard entries.
    /// </summary>
    [DataContract]
    public sealed class LeaderboardEntriesDTO
    {
        /// <summary>
        /// The Crypt of the NecroDancer leaderboard.
        /// </summary>
        [DataMember(Name = "leaderboard")]
        public LeaderboardDTO Leaderboard { get; set; }
        /// <summary>
        /// Total number of leaderboard entries.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of leaderboard entries.
        /// </summary>
        [DataMember(Name = "entries")]
        public IEnumerable<EntryDTO> Entries { get; set; }
    }
}