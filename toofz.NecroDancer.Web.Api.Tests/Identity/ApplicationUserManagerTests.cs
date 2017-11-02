using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Moq;
using toofz.NecroDancer.Web.Api.Identity;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Identity
{
    public class ApplicationUserManagerTests
    {
        public class CreateMethod
        {
            [Fact]
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
                Assert.IsAssignableFrom<ApplicationUserManager>(manager);
            }

            [Fact]
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
                Assert.NotNull(manager.UserTokenProvider);
            }
        }

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var store = Mock.Of<IUserStore<ApplicationUser>>();

                // Act
                var manager = new ApplicationUserManager(store);

                // Assert
                Assert.IsAssignableFrom<ApplicationUserManager>(manager);
            }
        }
    }
}
