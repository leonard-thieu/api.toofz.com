using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api;
using toofz.NecroDancer.Web.Api.Infrastructure;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace toofz.NecroDancer.Web.Api
{
    internal static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

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
        private static IKernel CreateKernel()
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
        internal static void RegisterServices(IKernel kernel)
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
            kernel.Bind<ProductsBinder>().ToMethod(s =>
            {
                using (var db = kernel.Get<ILeaderboardsContext>())
                {
                    var products = db.Products.Select(p => p.Name).ToList();

                    return new ProductsBinder(products);
                }
            });
            kernel.Bind<ModesBinder>().ToMethod(s =>
            {
                using (var db = kernel.Get<ILeaderboardsContext>())
                {
                    var modes = db.Modes.Select(p => p.Name).ToList();

                    return new ModesBinder(modes);
                }
            });
            kernel.Bind<RunsBinder>().ToMethod(s =>
            {
                using (var db = kernel.Get<ILeaderboardsContext>())
                {
                    var runs = db.Runs.Select(p => p.Name).ToList();

                    return new RunsBinder(runs);
                }
            });
            kernel.Bind<CharactersBinder>().ToMethod(s =>
            {
                using (var db = kernel.Get<ILeaderboardsContext>())
                {
                    var characters = db.Characters.Select(p => p.Name).ToList();

                    return new CharactersBinder(characters);
                }
            });
        }
    }
}
