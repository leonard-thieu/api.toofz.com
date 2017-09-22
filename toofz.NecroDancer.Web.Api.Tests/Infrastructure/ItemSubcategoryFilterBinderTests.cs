using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class ItemSubcategoryFilterBinderTests
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

            ItemSubcategoryFilterBinder binder = new ItemSubcategoryFilterBinder();
            HttpActionContext actionContext = null;
            string modelName = "filter";
            Mock<IValueProvider> mockValueProvider = new Mock<IValueProvider>();
            IValueProvider valueProvider;
            ModelBindingContext bindingContext;

            [TestMethod]
            public void CategoryIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.AreEqual(1, bindingContext.ModelState.Count);
            }

            [TestMethod]
            public void CategoryIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsFalse(success);
            }

            [TestMethod]
            public void WeaponsSubcategoryIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult("weapons"));
                mockValueProvider
                    .Setup(p => p.GetValue("subcategory"))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.AreEqual(1, bindingContext.ModelState.Count);
            }

            [TestMethod]
            public void WeaponsSubcategoryIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult("weapons"));
                mockValueProvider
                    .Setup(p => p.GetValue("subcategory"))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsFalse(success);
            }

            [TestMethod]
            public void ChestSubcategoryIsInvalid_AddsModelError()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult("chest"));
                mockValueProvider
                    .Setup(p => p.GetValue("subcategory"))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.AreEqual(1, bindingContext.ModelState.Count);
            }

            [TestMethod]
            public void ChestSubcategoryIsInvalid_ReturnsFalse()
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult("chest"));
                mockValueProvider
                    .Setup(p => p.GetValue("subcategory"))
                    .Returns(Util.CreateValueProviderResult("a"));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsFalse(success);
            }

            [DataTestMethod]
            [DataRow("weapons", "bows")]
            [DataRow("weapons", "broadswords")]
            [DataRow("weapons", "cats")]
            [DataRow("weapons", "crossbows")]
            [DataRow("weapons", "daggers")]
            [DataRow("weapons", "flails")]
            [DataRow("weapons", "longswords")]
            [DataRow("weapons", "rapiers")]
            [DataRow("weapons", "spears")]
            [DataRow("weapons", "whips")]
            [DataRow("chest", "red")]
            [DataRow("chest", "purple")]
            [DataRow("chest", "black")]
            [DataRow("chest", "mimic")]
            public void CategoryAndSubcategoryAreValid_SetsModel(string category, string subcategory)
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult(category));
                mockValueProvider
                    .Setup(p => p.GetValue("subcategory"))
                    .Returns(Util.CreateValueProviderResult(subcategory));

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                var filter = (ItemSubcategoryFilter)bindingContext.Model;
                Assert.AreEqual(category, filter.category);
                Assert.AreEqual(subcategory, filter.subcategory);
            }

            [DataTestMethod]
            [DataRow("weapons", "bows")]
            [DataRow("weapons", "broadswords")]
            [DataRow("weapons", "cats")]
            [DataRow("weapons", "crossbows")]
            [DataRow("weapons", "daggers")]
            [DataRow("weapons", "flails")]
            [DataRow("weapons", "longswords")]
            [DataRow("weapons", "rapiers")]
            [DataRow("weapons", "spears")]
            [DataRow("weapons", "whips")]
            [DataRow("chest", "red")]
            [DataRow("chest", "purple")]
            [DataRow("chest", "black")]
            [DataRow("chest", "mimic")]
            public void CategoryAndSubcategoryAreValid_ReturnsTrue(string category, string subcategory)
            {
                // Arrange
                mockValueProvider
                    .Setup(p => p.GetValue("category"))
                    .Returns(Util.CreateValueProviderResult(category));
                mockValueProvider
                    .Setup(p => p.GetValue("subcategory"))
                    .Returns(Util.CreateValueProviderResult(subcategory));

                // Act
                var success = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsTrue(success);
            }
        }
    }
}
