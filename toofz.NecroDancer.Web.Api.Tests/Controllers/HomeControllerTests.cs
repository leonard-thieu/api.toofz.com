using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Web.Api.Controllers;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    class HomeControllerTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var controller = new HomeController();

                // Assert
                Assert.IsInstanceOfType(controller, typeof(HomeController));
            }
        }

        [TestClass]
        public class IndexMethod
        {
            [TestMethod]
            public void ReturnsRedirect()
            {
                // Arrange
                var controller = new HomeController();

                // Act
                var result = controller.Index();

                // Assert
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            }
        }
    }
}
