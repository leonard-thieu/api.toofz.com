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
    public class CharactersBinderTests
    {
        public class Constructor
        {
            [Fact]
            public void LeaderboardCategoriesIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<string> characters = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new CharactersBinder(characters);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var characters = new List<string>();

                // Act
                var binder = new CharactersBinder(characters);

                // Assert
                Assert.IsAssignableFrom<CharactersBinder>(binder);
            }
        }

        public class GetModelMethod
        {
            [Fact]
            public void ReturnsModel()
            {
                // Arrange
                var characters = new[] { "cadence" };
                var binder = new CharactersBinder(characters);
                HttpActionContext actionContext = null;
                var modelName = "myModelName";
                var mockValueProvider = new Mock<IValueProvider>();
                mockValueProvider.Setup(v => v.GetValue("myModelName")).ReturnsValueProviderResult("cadence");
                var valueProvider = mockValueProvider.Object;
                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(Characters));
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
                Assert.IsAssignableFrom<Characters>(model);
            }
        }
    }
}
