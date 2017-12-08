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
    public class ModesBinderTests
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
                    new ModesBinder(values);
                });
            }

            [DisplayFact(nameof(ModesBinder))]
            public void ReturnsModesBinder()
            {
                // Arrange
                var values = new List<string>();

                // Act
                var binder = new ModesBinder(values);

                // Assert
                Assert.IsAssignableFrom<ModesBinder>(binder);
            }
        }

        public class GetModelMethod
        {
            [DisplayFact(nameof(Modes))]
            public void ReturnsModes()
            {
                // Arrange
                var values = new[] { "standard" };
                var binder = new ModesBinder(values);
                HttpActionContext actionContext = null;
                var modelName = "myModelName";
                var mockValueProvider = new Mock<IValueProvider>();
                mockValueProvider.Setup(v => v.GetValue("myModelName")).ReturnsValueProviderResult("standard");
                var valueProvider = mockValueProvider.Object;
                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(Modes));
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
                Assert.IsAssignableFrom<Modes>(model);
            }
        }
    }
}
