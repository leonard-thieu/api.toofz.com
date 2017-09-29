namespace toofz.NecroDancer.Web.Api.Models
{
    public interface IPagination
    {
        int Offset { get; set; }
        int Limit { get; set; }
    }
}