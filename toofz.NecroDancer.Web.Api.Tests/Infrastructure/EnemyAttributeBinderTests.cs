using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class EnemyAttributeBinderTests
    {
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

            private EnemyAttributeBinder binder = new EnemyAttributeBinder();
            private HttpActionContext actionContext = null;
            private string modelName = "myModelName";
            private Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            private IValueProvider valueProvider;
            private ModelBindingContext bindingContext;

            [Fact]
            public void ValueIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue(modelName)).ReturnsValueProviderResult("a");

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.Equal(1, bindingContext.ModelState.Count);
            }

            [Fact]
            public void ValueIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue(modelName)).ReturnsValueProviderResult("a");

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.False(success);
            }

            [Theory]
            [InlineData("boss")]
            [InlineData("bounce-on-movement-fail")]
            [InlineData("floating")]
            [InlineData("ignore-liquids")]
            [InlineData("ignore-walls")]
            [InlineData("is-monkey-like")]
            [InlineData("massive")]
            [InlineData("miniboss")]
            public void ValueIsValid_SetsModelToValue(string attribute)
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue(modelName)).ReturnsValueProviderResult(attribute);

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.Equal(attribute, bindingContext.Model);
            }

            [Theory]
            [InlineData("boss")]
            [InlineData("bounce-on-movement-fail")]
            [InlineData("floating")]
            [InlineData("ignore-liquids")]
            [InlineData("ignore-walls")]
            [InlineData("is-monkey-like")]
            [InlineData("massive")]
            [InlineData("miniboss")]
            public void ValueIsValid_ReturnsTrue(string attribute)
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue(modelName)).ReturnsValueProviderResult(attribute);

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.True(success);
            }
        }
    }
}
