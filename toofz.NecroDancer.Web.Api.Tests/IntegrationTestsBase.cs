using System.Web.Http;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using toofz.NecroDancer.Leaderboards;

namespace toofz.NecroDancer.Web.Api.Tests
{
    [TestCategory("Uses OWIN self hosting")]
    abstract class IntegrationTestsBase
    {
        protected TestServer Server { get; private set; }
        protected IKernel Kernel { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Kernel = NinjectWebCommon.CreateKernel();
            Kernel.Rebind<INecroDancerContext>().ToConstant(Mock.Of<INecroDancerContext>());
            Kernel.Rebind<ILeaderboardsContext>().ToConstant(Mock.Of<ILeaderboardsContext>());
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

        [TestCleanup]
        public void TestCleanup()
        {
            Server?.Dispose();
        }
    }
}
