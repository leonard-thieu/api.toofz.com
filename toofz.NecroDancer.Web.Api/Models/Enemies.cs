using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer enemies.
    /// </summary>
    public sealed class Enemies
    {
        /// <summary>
        /// The total number of enemies in the result set.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of enemies.
        /// </summary>
        public IEnumerable<Enemy> enemies { get; set; }
    }
}