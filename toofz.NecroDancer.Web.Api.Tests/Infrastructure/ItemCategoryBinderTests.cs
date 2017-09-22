using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class ItemCategoryBinderTests
    {
        [TestClass]
        public class BindModelMethod
        {
            public BindModelMethod()
            {
                valueProvider = mockValueProvider.Object;
                var modelMetadata = Util.CreateModelMetadata<string>();
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = valueProvider,
                    ModelMetadata = modelMetadata,
                };
            }

            ItemCategoryBinder binder = new ItemCategoryBinder();
            HttpActionContext actionContext = null;
            string modelName = "myModelName";
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            IValueProvider valueProvider;
            ModelBindingContext bindingContext;

            [TestMethod]
            public void ValueIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.AreEqual(1, bindingContext.ModelState.Count);
            }

            [TestMethod]
            public void ValueIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsFalse(success);
            }

            [DataTestMethod]
            [DataRow("armor")]
            [DataRow("consumable")]
            [DataRow("feet")]
            [DataRow("food")]
            [DataRow("head")]
            [DataRow("rings")]
            [DataRow("scrolls")]
            [DataRow("spells")]
            [DataRow("torches")]
            [DataRow("weapons")]
            public void ValueIsValid_SetsModelToValue(string category)
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult(category));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.AreEqual(category, bindingContext.Model);
            }

            [DataTestMethod]
            [DataRow("armor")]
            [DataRow("consumable")]
            [DataRow("feet")]
            [DataRow("food")]
            [DataRow("head")]
            [DataRow("rings")]
            [DataRow("scrolls")]
            [DataRow("spells")]
            [DataRow("torches")]
            [DataRow("weapons")]
            public void ValueIsValid_ReturnsTrue(string category)
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult(category));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsTrue(success);
            }
        }
    }
}
