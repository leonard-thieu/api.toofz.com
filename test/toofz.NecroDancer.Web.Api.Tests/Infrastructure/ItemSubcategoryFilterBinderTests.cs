using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class ItemSubcategoryFilterBinderTests
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

            private ItemSubcategoryFilterBinder binder = new ItemSubcategoryFilterBinder();
            private HttpActionContext actionContext = null;
            private string modelName = "filter";
            private Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            private IValueProvider valueProvider;
            private ModelBindingContext bindingContext;

            [DisplayFact]
            public void CategoryIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult("a");

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.Equal(1, bindingContext.ModelState.Count);
            }

            [DisplayFact]
            public void CategoryIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult("a");

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.False(success);
            }

            [DisplayFact]
            public void WeaponsSubcategoryIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult("weapons");
                mockValueProvider.Setup(p => p.GetValue("subcategory")).ReturnsValueProviderResult("a");

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.Equal(1, bindingContext.ModelState.Count);
            }

            [DisplayFact]
            public void WeaponsSubcategoryIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult("weapons");
                mockValueProvider.Setup(p => p.GetValue("subcategory")).ReturnsValueProviderResult("a");

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.False(success);
            }

            [DisplayFact]
            public void ChestSubcategoryIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult("chest");
                mockValueProvider.Setup(p => p.GetValue("subcategory")).ReturnsValueProviderResult("a");

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.Equal(1, bindingContext.ModelState.Count);
            }

            [DisplayFact]
            public void ChestSubcategoryIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult("chest");
                mockValueProvider.Setup(p => p.GetValue("subcategory")).ReturnsValueProviderResult("a");

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.False(success);
            }

            [DisplayTheory]
            [InlineData("weapons", "bows")]
            [InlineData("weapons", "broadswords")]
            [InlineData("weapons", "cats")]
            [InlineData("weapons", "crossbows")]
            [InlineData("weapons", "daggers")]
            [InlineData("weapons", "flails")]
            [InlineData("weapons", "longswords")]
            [InlineData("weapons", "rapiers")]
            [InlineData("weapons", "spears")]
            [InlineData("weapons", "whips")]
            [InlineData("chest", "red")]
            [InlineData("chest", "purple")]
            [InlineData("chest", "black")]
            [InlineData("chest", "mimic")]
            public void CategoryAndSubcategoryAreValid_SetsModel(string category, string subcategory)
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult(category);
                mockValueProvider.Setup(p => p.GetValue("subcategory")).ReturnsValueProviderResult(subcategory);

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                var filter = (ItemSubcategoryFilter)bindingContext.Model;
                Assert.Equal(category, filter.Category);
                Assert.Equal(subcategory, filter.Subcategory);
            }

            [DisplayTheory]
            [InlineData("weapons", "bows")]
            [InlineData("weapons", "broadswords")]
            [InlineData("weapons", "cats")]
            [InlineData("weapons", "crossbows")]
            [InlineData("weapons", "daggers")]
            [InlineData("weapons", "flails")]
            [InlineData("weapons", "longswords")]
            [InlineData("weapons", "rapiers")]
            [InlineData("weapons", "spears")]
            [InlineData("weapons", "whips")]
            [InlineData("chest", "red")]
            [InlineData("chest", "purple")]
            [InlineData("chest", "black")]
            [InlineData("chest", "mimic")]
            public void CategoryAndSubcategoryAreValid_ReturnsTrue(string category, string subcategory)
            {
                // Arrange
                mockValueProvider.Setup(p => p.GetValue("category")).ReturnsValueProviderResult(category);
                mockValueProvider.Setup(p => p.GetValue("subcategory")).ReturnsValueProviderResult(subcategory);

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.True(success);
            }
        }
    }
}
