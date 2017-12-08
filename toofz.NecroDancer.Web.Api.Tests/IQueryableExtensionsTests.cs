using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class IQueryableExtensionsTests
    {
        public class OrderByMethod
        {
            [DisplayFact(nameof(ArgumentNullException))]
            public void SourceIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IQueryable<string> source = null;
                var keySelectorMap = new Dictionary<string, string>();
                var sort = new List<string>();

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    source.OrderBy(keySelectorMap, sort);
                });
            }

            [DisplayFact(nameof(ArgumentNullException))]
            public void KeySelectorMapIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                IDictionary<string, string> keySelectorMap = null;
                var sort = new List<string>();

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    source.OrderBy(keySelectorMap, sort);
                });
            }

            [DisplayFact]
            public void SortIsNull_ReturnsSource()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var keySelectorMap = new Dictionary<string, string>();
                IEnumerable<string> sort = null;

                // Act
                var ordered = source.OrderBy(keySelectorMap, sort);

                // Assert
                Assert.Same(source, ordered);
            }

            [DisplayFact]
            public void SortIsEmpty_ReturnsSource()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var keySelectorMap = new Dictionary<string, string>();
                var sort = new List<string>();

                // Act
                var ordered = source.OrderBy(keySelectorMap, sort);

                // Assert
                Assert.Same(source, ordered);
            }

            [DisplayFact]
            public void ReturnsOrderedQuery()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var keySelectorMap = new Dictionary<string, string>
                {
                    ["option"] = "Length",
                };
                var sort = new List<string> { "option" };

                // Act
                var ordered = source.OrderBy(keySelectorMap, sort);

                // Assert
                Assert.NotSame(source, ordered);
            }
        }

        public class PageMethod
        {
            [DisplayFact(nameof(ArgumentNullException))]
            public void SourceIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IQueryable<string> source = null;
                var pagination = Mock.Of<IPagination>();

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    source.Page(pagination);
                });
            }

            [DisplayFact(nameof(ArgumentNullException))]
            public void PaginationIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                IPagination pagination = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    source.Page(pagination);
                });
            }

            [DisplayFact]
            public void ReturnsQuery()
            {
                // Arrange
                var source = (new List<string>()).AsQueryable();
                var pagination = Mock.Of<IPagination>();

                // Act
                var paginated = source.Page(pagination);

                // Assert
                Assert.IsAssignableFrom<IQueryable<string>>(paginated);
            }
        }
    }
}
