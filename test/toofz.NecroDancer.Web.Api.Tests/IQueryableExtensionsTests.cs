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
            [DisplayFact]
            public void ReturnsPaginatedQuery()
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
