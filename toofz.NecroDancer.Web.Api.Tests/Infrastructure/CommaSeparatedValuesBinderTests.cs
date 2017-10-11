﻿using System;
using System.Collections.Generic;
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
    class CommaSeparatedValuesBinderTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var binder = new CommaSeparatedValuesBinderAdapter();

                // Assert
                Assert.IsInstanceOfType(binder, typeof(CommaSeparatedValuesBinder));
            }

            sealed class CommaSeparatedValuesBinderAdapter : CommaSeparatedValuesBinder
            {
                protected override CommaSeparatedValues GetModel() => throw new NotImplementedException();
            }
        }

        [TestClass]
        public class BindModelMethod
        {
            public BindModelMethod()
            {
                var values = new[] { "item1", "item2", "item3" };
                var model = new CommaSeparatedValuesAdapter(values);
                binder = new CommaSeparatedValuesBinderAdapter(model);

                var data = new EmptyModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(CommaSeparatedValues));
                mockValueProvider = new Mock<IValueProvider>();
                var valueProvider = mockValueProvider.Object;
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = valueProvider,
                    ModelMetadata = modelMetadata,
                };
            }

            string modelName = "myModelName";
            CommaSeparatedValuesBinder binder;
            ModelBindingContext bindingContext;
            Mock<IValueProvider> mockValueProvider;

            [TestMethod]
            public void ValueIsNull_SetsModelWithDefaultValues()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).Returns((ValueProviderResult)null);

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(CommaSeparatedValues));
                var model = (CommaSeparatedValues)bindingContext.Model;
                var expected = new[] { "item1", "item2", "item3" };
                var actual = model.ToArray();
                CollectionAssert.AreEqual(expected, actual);
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
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,item3");

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(CommaSeparatedValues));
                var model = (CommaSeparatedValues)bindingContext.Model;
                var expected = new[] { "item1", "item3" };
                var actual = model.ToArray();
                CollectionAssert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void ValueIsValidCommaSeparatedValues_ReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,item3");

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void ValueContainsInvalidValues_AddsModelErrorsForInvalidValues()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,itemA,item3");

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue(modelName, out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
            }

            [TestMethod]
            public void ValueContainsInvalidValues_ReturnsFalse()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,itemA,item3");

                // Act
                var success = binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsFalse(success);
            }

            sealed class CommaSeparatedValuesBinderAdapter : CommaSeparatedValuesBinder
            {
                public CommaSeparatedValuesBinderAdapter(CommaSeparatedValues model)
                {
                    this.model = model;
                }

                readonly CommaSeparatedValues model;

                protected override CommaSeparatedValues GetModel() => model;
            }

            sealed class CommaSeparatedValuesAdapter : CommaSeparatedValues
            {
                public CommaSeparatedValuesAdapter(IEnumerable<string> defaults)
                {
                    this.defaults = defaults;
                }

                readonly IEnumerable<string> defaults;

                public override void Add(string item)
                {
                    if (!defaults.Contains(item))
                    {
                        throw new ArgumentException();
                    }
                    base.Add(item);
                }

                protected override IEnumerable<string> GetDefaults() => defaults;
            }
        }
    }
}
