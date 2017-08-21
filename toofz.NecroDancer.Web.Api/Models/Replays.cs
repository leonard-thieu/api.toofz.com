using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    public sealed class Replays
    {
        public int total { get; set; }
        public IEnumerable<Replay> replays { get; set; }
    }
}