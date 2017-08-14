using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    sealed class MockPagination : IPagination
    {
        [MinValue(0)]
        [MaxValue(30)]
        public int offset { get; set; } = 13;
        [MinValue(1)]
        [MaxValue(20)]
        public int limit { get; set; } = 7;
    }
}
