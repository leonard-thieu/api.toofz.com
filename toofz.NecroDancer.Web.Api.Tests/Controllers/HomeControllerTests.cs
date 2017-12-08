using System.Web.Mvc;
using toofz.NecroDancer.Web.Api.Controllers;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class HomeControllerTests
    {
        public class Constructor
        {
            [DisplayFact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var controller = new HomeController();

                // Assert
                Assert.IsAssignableFrom<HomeController>(controller);
            }
        }

        public class IndexMethod
        {
            [DisplayFact]
            public void ReturnsRedirect()
            {
                // Arrange
                var controller = new HomeController();

                // Act
                var result = controller.Index();

                // Assert
                Assert.IsAssignableFrom<RedirectToRouteResult>(result);
            }
        }
    }
}
