using System.Collections;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class CommaSeparatedValuesBinderTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var binder = new CommaSeparatedValuesBinder();

                // Assert
                Assert.IsInstanceOfType(binder, typeof(CommaSeparatedValuesBinder));
            }
        }

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

            CommaSeparatedValuesBinder binder = new CommaSeparatedValuesBinder();
            HttpActionContext actionContext = null;
            string modelName = "myModelName";
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            IValueProvider valueProvider;
            ModelBindingContext bindingContext;

            [TestMethod]
            public void ValueNotSpecified_ReturnsFalse()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns((ValueProviderResult)null);

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsFalse(success);
            }

            [TestMethod]
            public void ValueIsSpecified_SetsModelToSplitValues()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult("a,b,c"));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                var values = bindingContext.Model as ICollection;
                CollectionAssert.AreEquivalent(new[] { "a", "b", "c" }, values);
            }

            [TestMethod]
            public void ValueIsSpecified_ReturnsTrue()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue(modelName))
                    .Returns(Util.CreateValueProviderResult("a,b,c"));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsTrue(success);
            }
        }
    }
}
