using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using toofz.Data;
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

            kernel.Rebind<DbContextOptions<NecroDancerContext>>()
                  .ToMethod(c =>
                  {
                      var connectionString = StorageHelper.GetDatabaseConnectionString(Constants.NecroDancerContextName);

                      return new DbContextOptionsBuilder<NecroDancerContext>()
                        .UseSqlServer(connectionString)
                        .Options;
                  })
                  .WhenInjectedInto<NecroDancerContext>();

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

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                server.Dispose();
                kernel.Dispose();
            }
        }

        #endregion

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
        private static NecroDancerContext InitializeContext()
        {
            var connectionString = StorageHelper.GetDatabaseConnectionString(Constants.NecroDancerContextName);
            var options = new DbContextOptionsBuilder<NecroDancerContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var context = new NecroDancerContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
                context.EnsureSeedData();
            }

            return new NecroDancerContext(options);
        }

        public IntegrationTestsFixture()
        {
            Context = InitializeContext();
        }

        public NecroDancerContext Context { get; }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

    [CollectionDefinition(Name)]
    public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture>
    {
        public const string Name = "Integration tests collection";
    }
}
