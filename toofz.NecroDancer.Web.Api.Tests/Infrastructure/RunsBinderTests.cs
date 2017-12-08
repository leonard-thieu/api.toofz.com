using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class RunsBinderTests
    {
        public class Constructor
        {
            [DisplayFact(nameof(ArgumentNullException))]
            public void LeaderboardCategoriesIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<string> values = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new RunsBinder(values);
                });
            }

            [DisplayFact(nameof(RunsBinder))]
            public void ReturnsRunsBinder()
            {
                // Arrange
                var values = new List<string>();

                // Act
                var binder = new RunsBinder(values);

                // Assert
                Assert.IsAssignableFrom<RunsBinder>(binder);
            }
        }

        public class GetModelMethod
        {
            [DisplayFact(nameof(Runs))]
            public void ReturnsRuns()
            {
                // Arrange
                var values = new[] { "speed" };
                var binder = new RunsBinder(values);
                HttpActionContext actionContext = null;
                var modelName = "myModelName";
                var mockValueProvider = new Mock<IValueProvider>();
                mockValueProvider.Setup(v => v.GetValue("myModelName")).ReturnsValueProviderResult("speed");
                var valueProvider = mockValueProvider.Object;
                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(Runs));
                var bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = valueProvider,
                    ModelMetadata = modelMetadata,
                };

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                var model = bindingContext.Model;
                Assert.IsAssignableFrom<Runs>(model);
            }
        }
    }
}
