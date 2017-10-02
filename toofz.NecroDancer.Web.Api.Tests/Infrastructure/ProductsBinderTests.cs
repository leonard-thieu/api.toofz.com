using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class ProductsBinderTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void LeaderboardCategoriesIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<string> products = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new ProductsBinder(products);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var products = new List<string>();

                // Act
                var binder = new ProductsBinder(products);

                // Assert
                Assert.IsInstanceOfType(binder, typeof(ProductsBinder));
            }
        }

        [TestClass]
        public class GetModelMethod
        {
            [TestMethod]
            public void ReturnsModel()
            {
                // Arrange
                var products = new[] { "classic" };
                var binder = new ProductsBinder(products);
                HttpActionContext actionContext = null;
                var modelName = "myModelName";
                var mockValueProvider = new Mock<IValueProvider>();
                mockValueProvider.Setup(v => v.GetValue("myModelName")).Returns(Util.CreateValueProviderResult("classic"));
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
                Assert.IsInstanceOfType(model, typeof(Products));
            }
        }
    }
}
