using Owin;

namespace toofz.NecroDancer.Web.Api
{
    internal sealed partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
