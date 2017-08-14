using System.Globalization;
using System.Linq;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class LeaderboardCategoryBaseBinderTests
    {
        [TestClass]
        public class BindModel
        {
            const string modelName = "mockModelName";
            Category category;
            MockLeaderboardCategoryBaseBinder binder;
            ModelBindingContext bindingContext;
            Mock<IValueProvider> mock_ValueProvider;

            [TestInitialize]
            public void Initialize()
            {
                category = new Category
                {
                    { "item1", new CategoryItem() },
                    { "item2", new CategoryItem() },
                    { "item3", new CategoryItem() },
                };
                var model = new MockLeaderboardCategoryBase(category);
                binder = new MockLeaderboardCategoryBaseBinder(model);

                var data = new EmptyModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(LeaderboardCategoryBase));
                mock_ValueProvider = new Mock<IValueProvider>();
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = mock_ValueProvider.Object,
                    ModelMetadata = modelMetadata,
                };
            }

            [TestMethod]
            public void ValueIsNull_SetsModelWithAllValuesAndReturnsTrue()
            {
                // Arrange
                mock_ValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(LeaderboardCategoryBase));
                var model = (LeaderboardCategoryBase)bindingContext.Model;
                Assert.AreEqual(3, model.Count());
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void ValueIsEmptyString_SetsModelWithNoValuesAndReturnsTrue()
            {
                // Arrange
                mock_ValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns(new ValueProviderResult("", null, CultureInfo.InvariantCulture));

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(LeaderboardCategoryBase));
                var model = (LeaderboardCategoryBase)bindingContext.Model;
                Assert.AreEqual(0, model.Count());
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void ValueIsValidCommaSeparatedValues_SetsModelWithValuesAndReturnsTrue()
            {
                // Arrange
                mock_ValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns(new ValueProviderResult("item1,item3", null, CultureInfo.InvariantCulture));

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(LeaderboardCategoryBase));
                var model = (LeaderboardCategoryBase)bindingContext.Model;
                Assert.AreEqual(2, model.Count());
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void ValueContainsInvalidValues_AddsModelErrorsForInvalidValuesAndReturnsTrue()
            {
                // Arrange
                mock_ValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns(new ValueProviderResult("item1,itemA,item3", null, CultureInfo.InvariantCulture));

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue(modelName, out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
                Assert.IsTrue(canBind);
            }
        }
    }
}
