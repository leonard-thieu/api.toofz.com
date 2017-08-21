using System.Threading;
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
    class EnemiesControllerTests
    {
        [TestClass]
        public class GetEnemies
        {
            [TestMethod]
            public async Task ReturnsOkWithEnemies()
            {
                // Arrange
                var mockSet = MockHelper.MockSet<Data.Enemy>();

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var result = await controller.GetEnemies(new EnemiesPagination());

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Enemies>));
            }

            [TestMethod]
            public async Task WithAttribute_ReturnsOkWithEnemies()
            {
                // Arrange
                var mockSet = MockHelper.MockSet<Data.Enemy>();

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var result = await controller.GetEnemies(new EnemiesPagination(), "boss");

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Enemies>));
            }
        }

        [TestClass]
        public class GetEnemiesAsync
        {
            [TestMethod]
            public async Task ReturnsEnemies()
            {
                // Arrange
                var mockSet = MockHelper.MockSet<Data.Enemy>();

                var mockRepository = new Mock<NecroDancerContext>();
                mockRepository.Setup(x => x.Enemies).Returns(mockSet.Object);

                var controller = new EnemiesController(mockRepository.Object);

                // Act
                var enemies = await controller.GetEnemiesAsync(new EnemiesPagination(), null, CancellationToken.None);

                // Assert
                Assert.IsInstanceOfType(enemies, typeof(Enemies));
            }
        }
    }
}
