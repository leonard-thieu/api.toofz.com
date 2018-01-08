using System;
using System.Linq;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using toofz.Data;
using toofz.NecroDancer.Web.Api;
using toofz.NecroDancer.Web.Api.Infrastructure;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(NinjectWebCommon), nameof(NinjectWebCommon.Start))]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), nameof(NinjectWebCommon.Stop))]

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
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<TelemetryClient>()
                  .ToConstant(WebApiApplication.TelemetryClient);

            kernel.Bind<IExceptionLogger>()
                  .To<AiExceptionLogger>();

            kernel.Bind<DbContextOptions<NecroDancerContext>>()
                  .ToMethod(GetNecroDancerContextOptions)
                  .WhenInjectedInto<NecroDancerContext>();
            kernel.Bind<INecroDancerContext>()
                  .To<NecroDancerContext>();
            kernel.Bind<ILeaderboardsContext>()
                  .To<NecroDancerContext>();

            kernel.Bind<ProductsBinder>()
                  .ToMethod(GetProductsBinder);
            kernel.Bind<ModesBinder>()
                  .ToMethod(GetModesBinder);
            kernel.Bind<RunsBinder>()
                  .ToMethod(GetRunsBinder);
            kernel.Bind<CharactersBinder>()
                  .ToMethod(GetCharactersBinder);
        }

        private static DbContextOptions<NecroDancerContext> GetNecroDancerContextOptions(IContext c)
        {
            var connectionString = StorageHelper.GetDatabaseConnectionString(nameof(NecroDancerContext));

            return new DbContextOptionsBuilder<NecroDancerContext>()
              .UseSqlServer(connectionString)
              .Options;
        }

        private static ProductsBinder GetProductsBinder(IContext c)
        {
            using (var db = c.Kernel.Get<ILeaderboardsContext>())
            {
                return new ProductsBinder(db.Products.Select(p => p.Name));
            }
        }

        private static ModesBinder GetModesBinder(IContext c)
        {
            using (var db = c.Kernel.Get<ILeaderboardsContext>())
            {
                return new ModesBinder(db.Modes.Select(p => p.Name));
            }
        }

        private static RunsBinder GetRunsBinder(IContext c)
        {
            using (var db = c.Kernel.Get<ILeaderboardsContext>())
            {
                return new RunsBinder(db.Runs.Select(p => p.Name));
            }
        }

        private static CharactersBinder GetCharactersBinder(IContext c)
        {
            using (var db = c.Kernel.Get<ILeaderboardsContext>())
            {
                return new CharactersBinder(db.Characters.Select(p => p.Name));
            }
        }
    }
}
