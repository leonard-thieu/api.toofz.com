using System;
using System.Web.Http;
using Microsoft.Owin.Testing;
using Moq;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using toofz.NecroDancer.Leaderboards;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests
{
    [Trait("Category", "Uses OWIN self hosting")]
    public abstract class IntegrationTestsBase : IDisposable
    {
        public IntegrationTestsBase()
        {
            Kernel = new StandardKernel();
            Kernel.Unbind<HttpConfiguration>();
            NinjectWebCommon.RegisterServices(Kernel);
            Kernel.Rebind<INecroDancerContext>().ToConstant(Mock.Of<INecroDancerContext>());
            Kernel.Rebind<ILeaderboardsContext>().ToConstant(Util.CreateLeaderboardsContext());
            Kernel.Rebind<ILeaderboardsStoreClient>().ToConstant(Mock.Of<ILeaderboardsStoreClient>());
            Server = TestServer.Create(app =>
            {
                var config = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
                app.UseNinjectMiddleware(() => Kernel);
                app.UseNinjectWebApi(config);
                WebApiConfig.Register(config);
                app.UseWebApi(config);
            });
        }

        protected TestServer Server { get; private set; }
        protected IKernel Kernel { get; private set; }

        public void Dispose()
        {
            Server?.Dispose();
        }
    }
}
