using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class PaginationBinderTests
    {
        public class BindModel
        {
            public BindModel()
            {
                actionContext = ContextUtil.CreateActionContext();

                binder = new PaginationBinder<StubPagination>();

                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(IPagination));
                mockValueProvider = new Mock<IValueProvider>();
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = mockValueProvider.Object,
                    ModelMetadata = modelMetadata,
                };
            }

            private HttpActionContext actionContext;
            private const string modelName = "mockModelName";
            private PaginationBinder<StubPagination> binder;
            private ModelBindingContext bindingContext;
            private Mock<IValueProvider> mockValueProvider;

            [DisplayFact]
            public void OffsetValueIsNull_SetsModelWithDefaultOffsetValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsAssignableFrom<StubPagination>(bindingContext.Model);
                var model = (StubPagination)bindingContext.Model;
                Assert.Equal(13, model.Offset);
                Assert.True(canBind);
            }

            [DisplayFact]
            public void OffsetValueIsANumber_SetsModelWithOffsetValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult(3);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsAssignableFrom<StubPagination>(bindingContext.Model);
                var model = (StubPagination)bindingContext.Model;
                Assert.Equal(3, model.Offset);
                Assert.True(canBind);
            }

            [DisplayFact]
            public void OffsetValueIsNotANumber_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult("ten");

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("offset", out var modelState);
                Assert.True(hasErrors);
                Assert.Equal(1, modelState.Errors.Count);
                Assert.True(canBind);
            }

            [DisplayFact(Skip = "Determine how to properly set up for testing validation.")]
            public void OffsetValueIsLessThanMin_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult(int.MinValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("offset", out var modelState);
                Assert.True(hasErrors);
                Assert.Equal(1, modelState.Errors.Count);
                Assert.True(canBind);
            }

            [DisplayFact(Skip = "Determine how to properly set up for testing validation.")]
            public void OffsetValueIsMoreThanMax_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult(int.MaxValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("offset", out var modelState);
                Assert.True(hasErrors);
                Assert.Equal(1, modelState.Errors.Count);
                Assert.True(canBind);
            }

            [DisplayFact]
            public void LimitValueIsNull_SetsModelWithDefaultLimitValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsAssignableFrom<StubPagination>(bindingContext.Model);
                var model = (StubPagination)bindingContext.Model;
                Assert.Equal(7, model.Limit);
                Assert.True(canBind);
            }

            [DisplayFact]
            public void LimitValueIsANumber_SetsModelWithLimitValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult(3);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsAssignableFrom<StubPagination>(bindingContext.Model);
                var model = (StubPagination)bindingContext.Model;
                Assert.Equal(3, model.Limit);
                Assert.True(canBind);
            }

            [DisplayFact]
            public void LimitValueIsNotANumber_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult("ten");

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("limit", out var modelState);
                Assert.True(hasErrors);
                Assert.Equal(1, modelState.Errors.Count);
                Assert.True(canBind);
            }

            [DisplayFact(Skip = "Determine how to properly set up for testing validation.")]
            public void LimitValueIsLessThanMin_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult(int.MinValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("limit", out var modelState);
                Assert.True(hasErrors);
                Assert.Equal(1, modelState.Errors.Count);
                Assert.True(canBind);
            }

            [DisplayFact(Skip = "Determine how to properly set up for testing validation.")]
            public void LimitValueIsMoreThanMax_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult(int.MaxValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("limit", out var modelState);
                Assert.True(hasErrors);
                Assert.Equal(1, modelState.Errors.Count);
                Assert.True(canBind);
            }

            [DisplayFact]
            public void OffsetValueAndLimitValueAreNull_SetsModelWithDefaultValuesAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).Returns((ValueProviderResult)null);
                mockValueProvider.Setup(v => v.GetValue("offset")).Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsAssignableFrom<StubPagination>(bindingContext.Model);
                var model = (StubPagination)bindingContext.Model;
                Assert.Equal(7, model.Limit);
                Assert.Equal(13, model.Offset);
                Assert.True(canBind);
            }

            private sealed class StubPagination : IPagination
            {
                [MinValue(0)]
                [MaxValue(30)]
                public int Offset { get; set; } = 13;
                [MinValue(1)]
                [MaxValue(20)]
                public int Limit { get; set; } = 7;
            }
        }
    }
}
