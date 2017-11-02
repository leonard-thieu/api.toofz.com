using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace toofz.NecroDancer.Web.Api.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public ApplicationDbContext()
        {
            Initialize();
        }

        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString, throwIfV1Schema: false)
        {
            Initialize();
        }

        private void Initialize()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
    }
}