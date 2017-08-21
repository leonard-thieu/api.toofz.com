namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer enemy.
    /// </summary>
    public sealed class Enemy
    {
        /// <summary>
        /// The enemy's element name.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The enemy's type.
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// The enemy's display name.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// The enemy's health.
        /// </summary>
        public int health { get; set; }
        /// <summary>
        /// The amount of damage that the enemy does per attack.
        /// </summary>
        public int damage { get; set; }
        /// <summary>
        /// The enemy's beats per move.
        /// </summary>
        public int beats_per_move { get; set; }
        /// <summary>
        /// The base amount of coins the enemy drops when killed.
        /// </summary>
        public int drops { get; set; }
    }
}