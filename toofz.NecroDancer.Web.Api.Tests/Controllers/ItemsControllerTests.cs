using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.EntityFramework;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    class ItemsControllerTests
    {
        [TestClass]
        public class GetItems
        {
            [TestMethod]
            public async Task ReturnsOkWithItems()
            {
                // Arrange
                var mockItems = MockHelper.MockSet<Data.Item>();
                var mockDb = new Mock<NecroDancerContext>();
                mockDb
                    .SetupGet(db => db.Items)
                    .Returns(mockItems.Object);
                var controller = new ItemsController(mockDb.Object);
                var pagination = new ItemsPagination();

                // Act
                var result = await controller.GetItems(pagination);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Items>));
            }

            [TestMethod]
            public async Task WithCategory_ReturnsOkWithItems()
            {
                // Arrange
                var mockItems = MockHelper.MockSet<Data.Item>();
                var mockDb = new Mock<NecroDancerContext>();
                mockDb
                    .SetupGet(db => db.Items)
                    .Returns(mockItems.Object);
                var controller = new ItemsController(mockDb.Object);
                var pagination = new ItemsPagination();
                var category = "armor";

                // Act
                var result = await controller.GetItems(pagination, category);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Items>));
            }

            [TestMethod]
            public async Task WithCategoryAndsubcategory_ReturnsOkWithItems()
            {
                // Arrange
                var mockItems = MockHelper.MockSet<Data.Item>();
                var mockDb = new Mock<NecroDancerContext>();
                mockDb
                    .SetupGet(db => db.Items)
                    .Returns(mockItems.Object);
                var controller = new ItemsController(mockDb.Object);
                var pagination = new ItemsPagination();
                var category = "weapons";
                var subcategory = "bows";

                // Act
                var result = await controller.GetItems(pagination, category, subcategory);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Items>));
            }
        }
    }
}
