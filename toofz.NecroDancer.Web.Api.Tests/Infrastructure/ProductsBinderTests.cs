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
    public class ProductsBinderTests
    {
        public class Constructor
        {
            [Fact]
            public void LeaderboardCategoriesIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<string> products = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new ProductsBinder(products);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var products = new List<string>();

                // Act
                var binder = new ProductsBinder(products);

                // Assert
                Assert.IsAssignableFrom<ProductsBinder>(binder);
            }
        }

        public class GetModelMethod
        {
            [Fact]
            public void ReturnsModel()
            {
                // Arrange
                var products = new[] { "classic" };
                var binder = new ProductsBinder(products);
                HttpActionContext actionContext = null;
                var modelName = "myModelName";
                var mockValueProvider = new Mock<IValueProvider>();
                mockValueProvider.Setup(v => v.GetValue("myModelName")).ReturnsValueProviderResult("classic");
                var valueProvider = mockValueProvider.Object;
                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(Products));
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
                Assert.IsAssignableFrom<Products>(model);
            }
        }
    }
}
