using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Identity;

namespace toofz.NecroDancer.Web.Api.Tests.Identity
{
    class ApplicationUserManagerTests
    {
        [TestClass]
        public class CreateMethod
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var options = new IdentityFactoryOptions<ApplicationUserManager>();
                var mockContext = new Mock<IOwinContext>();
                mockContext.Setup(c => c.Get<ApplicationDbContext>(It.IsAny<string>())).Returns(new ApplicationDbContext());
                var context = mockContext.Object;

                // Act
                var manager = ApplicationUserManager.Create(options, context);

                // Assert
                Assert.IsInstanceOfType(manager, typeof(ApplicationUserManager));
            }
        }

        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var store = Mock.Of<IUserStore<ApplicationUser>>();

                // Act
                var manager = new ApplicationUserManager(store);

                // Assert
                Assert.IsInstanceOfType(manager, typeof(ApplicationUserManager));
            }
        }
    }
}
