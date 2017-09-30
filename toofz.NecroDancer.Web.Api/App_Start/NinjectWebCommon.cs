using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace toofz.NecroDancer.Web.Api
{
    [ExcludeFromCodeCoverage]
    static class NinjectWebCommon
    {
        static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        internal static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<System.Web.IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        static void RegisterServices(IKernel kernel)
        {
            var necroDancerConnectionString = Util.GetEnvVar("NecroDancerConnectionString");
            var leaderboardsConnectionString = Util.GetEnvVar("LeaderboardsConnectionString");

            kernel.Bind<INecroDancerContext>().ToConstructor(s => new NecroDancerContext(necroDancerConnectionString));
            kernel.Bind<ILeaderboardsContext>().ToConstructor(s => new LeaderboardsContext(leaderboardsConnectionString));
            kernel.Bind<SqlConnection>().ToMethod(s =>
            {
                var connection = new SqlConnection(leaderboardsConnectionString);
                connection.Open();

                return connection;
            });
            kernel.Bind<ILeaderboardsStoreClient>().ToConstructor(s => new LeaderboardsStoreClient(s.Inject<SqlConnection>()));
            kernel.Bind<Categories>().ToMethod(s => LeaderboardsResources.ReadLeaderboardCategories());
            kernel.Bind<LeaderboardHeaders>().ToMethod(s => LeaderboardsResources.ReadLeaderboardHeaders());
        }
    }
}
