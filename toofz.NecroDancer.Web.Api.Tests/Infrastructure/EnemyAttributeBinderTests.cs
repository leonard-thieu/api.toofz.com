using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class EnemyAttributeBinderTests
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

            EnemyAttributeBinder binder = new EnemyAttributeBinder();
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
            [DataRow("boss")]
            [DataRow("bounce-on-movement-fail")]
            [DataRow("floating")]
            [DataRow("ignore-liquids")]
            [DataRow("ignore-walls")]
            [DataRow("is-monkey-like")]
            [DataRow("massive")]
            [DataRow("miniboss")]
            public void ValueIsValid_SetsModelToValue(string attribute)
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult(attribute));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.AreEqual(attribute, bindingContext.Model);
            }

            [DataTestMethod]
            [DataRow("boss")]
            [DataRow("bounce-on-movement-fail")]
            [DataRow("floating")]
            [DataRow("ignore-liquids")]
            [DataRow("ignore-walls")]
            [DataRow("is-monkey-like")]
            [DataRow("massive")]
            [DataRow("miniboss")]
            public void ValueIsValid_ReturnsTrue(string attribute)
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult(attribute));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsTrue(success);
            }
        }
    }
}
