using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class ItemCategoryBinderTests
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

            private ItemCategoryBinder binder = new ItemCategoryBinder();
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
            [InlineData("armor")]
            [InlineData("consumable")]
            [InlineData("feet")]
            [InlineData("food")]
            [InlineData("head")]
            [InlineData("rings")]
            [InlineData("scrolls")]
            [InlineData("spells")]
            [InlineData("torches")]
            [InlineData("weapons")]
            public void ValueIsValid_SetsModelToValue(string category)
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue(modelName)).ReturnsValueProviderResult(category);

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.Equal(category, bindingContext.Model);
            }

            [Theory]
            [InlineData("armor")]
            [InlineData("consumable")]
            [InlineData("feet")]
            [InlineData("food")]
            [InlineData("head")]
            [InlineData("rings")]
            [InlineData("scrolls")]
            [InlineData("spells")]
            [InlineData("torches")]
            [InlineData("weapons")]
            public void ValueIsValid_ReturnsTrue(string category)
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue(modelName)).ReturnsValueProviderResult(category);

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.True(success);
            }
        }
    }
}
