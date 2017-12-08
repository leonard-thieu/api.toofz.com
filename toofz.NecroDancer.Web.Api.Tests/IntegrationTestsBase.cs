using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using toofz.Data;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace toofz.NecroDancer.Web.Api.Tests
{
    [Trait("Category", "Uses OWIN self hosting")]
    [Trait("Category", "Uses SQL Server")]
    [Collection(IntegrationTestsCollection.Name)]
    public abstract class IntegrationTestsBase : IDisposable
    {
        public IntegrationTestsBase(IntegrationTestsFixture fixture, ITestOutputHelper output)
        {
            this.output = output;

            kernel.Unbind<HttpConfiguration>();

            kernel.Rebind<string>()
                  .ToMethod(c => StorageHelper.GetDatabaseConnectionString(nameof(NecroDancerContext)))
                  .WhenInjectedInto<NecroDancerContext>();

            kernel.Rebind<string>()
                  .ToMethod(c => StorageHelper.GetDatabaseConnectionString(nameof(LeaderboardsContext)))
                  .WhenInjectedInto<LeaderboardsContext>();

            server = TestServer.Create(app =>
            {
                var config = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
                app.UseNinjectMiddleware(() => kernel);
                app.UseNinjectWebApi(config);
                WebApiConfig.Register(config);
                app.UseWebApi(config);
            });
        }

        protected readonly ITestOutputHelper output;
        private readonly IKernel kernel = NinjectWebCommon.CreateKernel();
        protected readonly TestServer server;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                server?.Dispose();
                kernel.Dispose();
            }
        }

        public async Task RespondsWithAsync(
            HttpResponseMessage response,
            string expectedContent,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                var httpError = JsonConvert.DeserializeObject<HttpError>(content);
                while (httpError != null)
                {
                    output.WriteLine($"[{httpError.ExceptionType}] {httpError.ExceptionMessage}");
                    output.WriteLine(httpError.StackTrace);
                    httpError = httpError.InnerException;
                }
            }
            catch (JsonSerializationException)
            {
                output.WriteLine(content);
            }

            var messages = new List<object>();

            try
            {
                Assert.Equal(expectedStatusCode, response.StatusCode);
            }
            catch (Exception ex)
            {
                messages.Add(ex);
            }

            try
            {
                Assert.Equal(expectedContent, content, ignoreLineEndingDifferences: true);
            }
            catch (Exception ex)
            {
                messages.Add(ex);
            }

            if (messages.Any())
            {
                throw new XunitException(string.Join(Environment.NewLine, messages));
            }
        }

        [DataContract]
        private sealed class HttpError
        {
            [DataMember(Name = "message")]
            public string Message { get; set; }
            [DataMember(Name = "messageDetail")]
            public string MessageDetail { get; set; }
            [DataMember(Name = "exceptionMessage", IsRequired = true)]
            public string ExceptionMessage { get; set; }
            [DataMember(Name = "exceptionType", IsRequired = true)]
            public string ExceptionType { get; set; }
            [DataMember(Name = "stackTrace", IsRequired = true)]
            public string StackTrace { get; set; }
            [DataMember(Name = "innerException")]
            public HttpError InnerException { get; set; }
        }
    }

    public class IntegrationTestsFixture : IDisposable
    {
        public IntegrationTestsFixture()
        {
            NecroDancerDb = new NecroDancerContext(StorageHelper.GetDatabaseConnectionString(nameof(NecroDancerContext)));
            NecroDancerDb.Database.Delete(); // Make sure it really dropped - needed for dirty database
            Database.SetInitializer(new TestNecroDancerContextDatabaseInitializer());
            NecroDancerDb.Database.Initialize(force: true);

            LeaderboardsDb = new LeaderboardsContext(StorageHelper.GetDatabaseConnectionString(nameof(LeaderboardsContext)));
            LeaderboardsDb.Database.Delete(); // Make sure it really dropped - needed for dirty database
            Database.SetInitializer(new TestLeaderboardsContextDatabaseInitializer());
            LeaderboardsDb.Database.Initialize(force: true);
        }

        public NecroDancerContext NecroDancerDb { get; }
        public LeaderboardsContext LeaderboardsDb { get; }

        public void Dispose()
        {
            LeaderboardsDb?.Database.Delete();
            NecroDancerDb?.Database.Delete();
        }

        private sealed class TestNecroDancerContextDatabaseInitializer : CreateDatabaseIfNotExists<NecroDancerContext>
        {
            protected override void Seed(NecroDancerContext context)
            {
                var items = JsonConvert.DeserializeObject<IEnumerable<Data.Item>>(NecroDancerResources.Items);
                context.Items.AddRange(items);
                var enemies = JsonConvert.DeserializeObject<IEnumerable<Data.Enemy>>(NecroDancerResources.Enemies);
                context.Enemies.AddRange(enemies);

                context.SaveChanges();
            }
        }

        private sealed class TestLeaderboardsContextDatabaseInitializer : CreateDatabaseIfNotExists<LeaderboardsContext>
        {
            protected override void Seed(LeaderboardsContext context)
            {
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(LeaderboardsResources.Products);
                context.Products.AddRange(products);
                var modes = JsonConvert.DeserializeObject<IEnumerable<Mode>>(LeaderboardsResources.Modes);
                context.Modes.AddRange(modes);
                var runs = JsonConvert.DeserializeObject<IEnumerable<Run>>(LeaderboardsResources.Runs);
                context.Runs.AddRange(runs);
                var characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(LeaderboardsResources.Characters);
                context.Characters.AddRange(characters);
                var leaderboards = JsonConvert.DeserializeObject<IEnumerable<Leaderboard>>(LeaderboardsResources.Leaderboards);
                context.Leaderboards.AddRange(leaderboards);
                var entries = JsonConvert.DeserializeObject<IEnumerable<Entry>>(LeaderboardsResources.Entries);
                context.Entries.AddRange(entries);
                var dailyLeaderboards = JsonConvert.DeserializeObject<IEnumerable<DailyLeaderboard>>(LeaderboardsResources.DailyLeaderboards);
                context.DailyLeaderboards.AddRange(dailyLeaderboards);
                var dailyEntries = JsonConvert.DeserializeObject<IEnumerable<DailyEntry>>(LeaderboardsResources.DailyEntries);
                context.DailyEntries.AddRange(dailyEntries);
                var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(LeaderboardsResources.Players);
                context.Players.AddRange(players);
                var replays = JsonConvert.DeserializeObject<IEnumerable<Replay>>(LeaderboardsResources.Replays);
                context.Replays.AddRange(replays);

                context.SaveChanges();
            }
        }
    }

    [CollectionDefinition(Name)]
    public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture>
    {
        public const string Name = "Integration tests collection";
    }
}
