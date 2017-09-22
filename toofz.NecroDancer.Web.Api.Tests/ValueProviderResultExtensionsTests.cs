using System;
using System.Globalization;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Web.Api.Tests
{
    class ValueProviderResultExtensionsTests
    {
        [TestClass]
        public class ConvertToMethod
        {
            [TestMethod]
            public void ResultIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                ValueProviderResult result = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    ValueProviderResultExtensions.ConvertTo<object>(result);
                });
            }

            [TestMethod]
            public void ValueIsConvertibleToType_ReturnsValueAsType()
            {
                // Arrange
                var result = new ValueProviderResult(15, "15", CultureInfo.InvariantCulture);

                // Act
                var value = result.ConvertTo<int>();

                // Assert
                Assert.AreEqual(15, value);
            }

            [TestMethod]
            public void ValueIsNotConvertibleToType_ReturnsDefaultOfType()
            {
                // Arrange
                var result = new ValueProviderResult(-15, "-15", CultureInfo.InvariantCulture);

                // Act
                var value = result.ConvertTo<uint>();

                // Assert
                Assert.AreEqual(default, value);
            }
        }
    }
}
