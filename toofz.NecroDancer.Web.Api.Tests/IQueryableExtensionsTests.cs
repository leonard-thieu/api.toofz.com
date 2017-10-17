using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Models;

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

        [TestClass]
        public class PageMethod
        {
            [TestMethod]
            public void SourceIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IQueryable<string> source = null;
                var pagination = Mock.Of<IPagination>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    source.Page(pagination);
                });
            }

            [TestMethod]
            public void PaginationIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                IPagination pagination = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    source.Page(pagination);
                });
            }

            [TestMethod]
            public void ReturnsQueryable()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var pagination = Mock.Of<IPagination>();

                // Act
                var paginated = source.Page(pagination);

                // Assert
                Assert.IsInstanceOfType(paginated, typeof(IQueryable<string>));
            }
        }
    }
}
