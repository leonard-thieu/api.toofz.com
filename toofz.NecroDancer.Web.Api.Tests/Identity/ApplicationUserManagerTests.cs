using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
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

            [TestMethod]
            public void DataProtectionProviderIsSet_SetsUserTokenProvider()
            {
                // Arrange
                var options = new IdentityFactoryOptions<ApplicationUserManager>();
                var mockDataProtectionProvider = new Mock<IDataProtectionProvider>();
                mockDataProtectionProvider.Setup(p => p.Create(It.IsAny<string>())).Returns(Mock.Of<IDataProtector>());
                var dataProtectionProvider = mockDataProtectionProvider.Object;
                options.DataProtectionProvider = dataProtectionProvider;
                var mockContext = new Mock<IOwinContext>();
                mockContext.Setup(c => c.Get<ApplicationDbContext>(It.IsAny<string>())).Returns(new ApplicationDbContext());
                var context = mockContext.Object;

                // Act
                var manager = ApplicationUserManager.Create(options, context);

                // Assert
                Assert.IsNotNull(manager.UserTokenProvider);
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
