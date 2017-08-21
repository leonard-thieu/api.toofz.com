using Newtonsoft.Json;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer leaderboard entry.
    /// </summary>
    public sealed class Entry
    {
        /// <summary>
        /// The leaderboard that contains the entry.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Leaderboard leaderboard { get; set; }
        /// <summary>
        /// The player that submitted the entry.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Player player { get; set; }
        /// <summary>
        /// The rank of the entry on the leaderboard.
        /// </summary>
        public int rank { get; set; }
        /// <summary>
        /// The raw score value.
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// The zone and level that the entry ends on.
        /// </summary>
        public End end { get; set; }
        /// <summary>
        /// The entity that killed the player. This may be null if the replay has not been parsed yet or 
        /// there was an error parsing the replay.
        /// </summary>
        public string killed_by { get; set; }
        /// <summary>
        /// Version of the entry's replay. This may be null if the replay has not been parsed yet or 
        /// there was an error parsing the replay.
        /// </summary>
        public int? version { get; set; }
    }
}