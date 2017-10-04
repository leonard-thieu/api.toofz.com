using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    [DataContract]
    public sealed class DailyEntryDTO
    {
        /// <summary>
        /// The leaderboard that contains the entry.
        /// </summary>
        [DataMember(Name = "leaderboard", EmitDefaultValue = false)]
        public DailyLeaderboardDTO Leaderboard { get; set; }
        /// <summary>
        /// The player that submitted the entry.
        /// </summary>
        [DataMember(Name = "player", EmitDefaultValue = false)]
        public PlayerDTO Player { get; set; }
        /// <summary>
        /// The rank of the entry on the leaderboard.
        /// </summary>
        [DataMember(Name = "rank")]
        public int Rank { get; set; }
        /// <summary>
        /// The raw score value.
        /// </summary>
        [DataMember(Name = "score")]
        public int Score { get; set; }
        /// <summary>
        /// The zone and level that the entry ends on.
        /// </summary>
        [DataMember(Name = "end")]
        public EndDTO End { get; set; }
        /// <summary>
        /// The entity that killed the player. This may be null if the replay has not been parsed yet or 
        /// there was an error parsing the replay.
        /// </summary>
        [DataMember(Name = "killed_by")]
        public string KilledBy { get; set; }
        /// <summary>
        /// Version of the entry's replay. This may be null if the replay has not been parsed yet or 
        /// there was an error parsing the replay.
        /// </summary>
        [DataMember(Name = "version")]
        public int? Version { get; set; }
    }
}