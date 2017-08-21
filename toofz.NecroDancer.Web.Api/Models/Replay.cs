namespace toofz.NecroDancer.Web.Api.Models
{
    public sealed class Replay
    {
        public string id { get; set; }
        public int? error { get; set; }
        public int? seed { get; set; }
        public int? version { get; set; }
        public string killed_by { get; set; }
    }
}