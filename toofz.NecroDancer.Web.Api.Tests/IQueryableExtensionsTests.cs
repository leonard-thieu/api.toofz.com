using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Web.Api.Tests
{
    class IQueryableExtensionsTests
    {
        [TestClass]
        public class OrderByMethod
        {
            [TestMethod]
            public void SourceIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IQueryable<string> source = null;
                var keySelectorMap = new Dictionary<string, string>();
                var sort = new List<string>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    IQueryableExtensions.OrderBy(source, keySelectorMap, sort);
                });
            }

            [TestMethod]
            public void KeySelectorMapIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                IDictionary<string, string> keySelectorMap = null;
                var sort = new List<string>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    IQueryableExtensions.OrderBy(source, keySelectorMap, sort);
                });
            }

            [TestMethod]
            public void SortIsNull_ReturnsSource()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var keySelectorMap = new Dictionary<string, string>();
                IEnumerable<string> sort = null;

                // Act
                var ordered = IQueryableExtensions.OrderBy(source, keySelectorMap, sort);

                // Assert
                Assert.AreSame(source, ordered);
            }

            [TestMethod]
            public void SortIsEmpty_ReturnsSource()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var keySelectorMap = new Dictionary<string, string>();
                var sort = new List<string>();

                // Act
                var ordered = IQueryableExtensions.OrderBy(source, keySelectorMap, sort);

                // Assert
                Assert.AreSame(source, ordered);
            }

            [TestMethod]
            public void ReturnsQueryableWithOrdering()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var keySelectorMap = new Dictionary<string, string>
                {
                    ["option"] = "Length",
                };
                var sort = new List<string> { "option" };

                // Act
                var ordered = IQueryableExtensions.OrderBy(source, keySelectorMap, sort);

                // Assert
                Assert.AreNotSame(source, ordered);
            }
        }
    }
}
