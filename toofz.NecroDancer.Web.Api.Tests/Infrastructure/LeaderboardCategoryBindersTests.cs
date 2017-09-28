using System;
using System.Globalization;
using System.Linq;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class LeaderboardCategoryBaseBinderTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var binder = new LeaderboardCategoryBaseBinderAdapter();

                // Assert
                Assert.IsInstanceOfType(binder, typeof(LeaderboardCategoryBaseBinder));
            }

            sealed class LeaderboardCategoryBaseBinderAdapter : LeaderboardCategoryBaseBinder
            {
                protected override LeaderboardCategoryBase GetModel()
                {
                    throw new NotImplementedException();
                }
            }
        }

        [TestClass]
        public class BindModelMethod
        {
            public BindModelMethod()
            {
                category = new Category
                {
                    { "item1", new CategoryItem() },
                    { "item2", new CategoryItem() },
                    { "item3", new CategoryItem() },
                };
                var model = new LeaderboardCategoryBaseAdapter(category);
                binder = new LeaderboardCategoryBaseBinderAdapter(model);

                var data = new EmptyModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(LeaderboardCategoryBase));
                mockValueProvider = new Mock<IValueProvider>();
                var valueProvider = mockValueProvider.Object;
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = valueProvider,
                    ModelMetadata = modelMetadata,
                };
            }

            const string modelName = "myModelName";
            Category category;
            LeaderboardCategoryBaseBinderAdapter binder;
            ModelBindingContext bindingContext;
            Mock<IValueProvider> mockValueProvider;

            [TestMethod]
            public void ValueIsNull_SetsModelWithAllValues()
            {
                // Arrange
                mockValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns((ValueProviderResult)null);

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(LeaderboardCategoryBase));
                var model = (LeaderboardCategoryBase)bindingContext.Model;
                Assert.AreEqual(3, model.Count());
            }

            [TestMethod]
            public void ValueIsNull_ReturnsTrue()
            {
                // Arrange
                mockValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void ValueIsValidCommaSeparatedValues_SetsModelWithValues()
            {
                // Arrange
                mockValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns(new ValueProviderResult("item1,item3", null, CultureInfo.InvariantCulture));

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(LeaderboardCategoryBase));
                var model = (LeaderboardCategoryBase)bindingContext.Model;
                Assert.AreEqual(2, model.Count());
            }

            [TestMethod]
            public void ValueIsValidCommaSeparatedValues_ReturnsTrue()
            {
                // Arrange
                mockValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns(new ValueProviderResult("item1,item3", null, CultureInfo.InvariantCulture));

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void ValueContainsInvalidValues_AddsModelErrorsForInvalidValues()
            {
                // Arrange
                mockValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns(new ValueProviderResult("item1,itemA,item3", null, CultureInfo.InvariantCulture));

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue(modelName, out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
            }

            [TestMethod]
            public void ValueContainsInvalidValues_ReturnsFalse()
            {
                // Arrange
                mockValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns(new ValueProviderResult("item1,itemA,item3", null, CultureInfo.InvariantCulture));

                // Act
                var success = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsFalse(success);
            }

            sealed class LeaderboardCategoryBaseBinderAdapter : LeaderboardCategoryBaseBinder
            {
                public LeaderboardCategoryBaseBinderAdapter(LeaderboardCategoryBase model)
                {
                    this.model = model;
                }

                readonly LeaderboardCategoryBase model;

                protected override LeaderboardCategoryBase GetModel()
                {
                    return model;
                }
            }

            sealed class LeaderboardCategoryBaseAdapter : LeaderboardCategoryBase
            {
                public LeaderboardCategoryBaseAdapter(Category category) : base(category) { }
            }
        }
    }
}
