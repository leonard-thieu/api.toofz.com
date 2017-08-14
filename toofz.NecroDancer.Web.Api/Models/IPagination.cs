namespace toofz.NecroDancer.Web.Api.Models
{
    public interface IPagination
    {
        int offset { get; set; }
        int limit { get; set; }
    }
}