using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer enemy.
    /// </summary>
    [DataContract]
    public sealed class EnemyDTO
    {
        /// <summary>
        /// The enemy's element name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// The enemy's type.
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }
        /// <summary>
        /// The enemy's display name.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
        /// <summary>
        /// The enemy's health.
        /// </summary>
        [DataMember(Name = "health")]
        public int Health { get; set; }
        /// <summary>
        /// The amount of damage that the enemy does per attack.
        /// </summary>
        [DataMember(Name = "damage")]
        public int Damage { get; set; }
        /// <summary>
        /// The enemy's beats per move.
        /// </summary>
        [DataMember(Name = "beats_per_move")]
        public int BeatsPerMove { get; set; }
        /// <summary>
        /// The base amount of coins the enemy drops when killed.
        /// </summary>
        [DataMember(Name = "drops")]
        public int Drops { get; set; }
    }
}