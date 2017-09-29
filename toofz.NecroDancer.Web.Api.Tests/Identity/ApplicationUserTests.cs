using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Identity;

namespace toofz.NecroDancer.Web.Api.Tests.Identity
{
    class ApplicationUserTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var user = new ApplicationUser();

                // Assert
                Assert.IsInstanceOfType(user, typeof(ApplicationUser));
            }
        }

        [TestClass]
        public class GenerateUserIdentityAsyncMethod
        {
            [TestMethod]
            public async Task ReturnsClaimsIdentity()
            {
                // Arrange
                var user = new ApplicationUser { UserName = "myUserName" };
                var store = Mock.Of<IUserStore<ApplicationUser>>();
                var manager = new ApplicationUserManager(store);
                var authenticationType = "myAuthenticationType";

                // Act
                var claimsIdentity = await user.GenerateUserIdentityAsync(manager, authenticationType);

                // Assert
                Assert.IsInstanceOfType(claimsIdentity, typeof(ClaimsIdentity));
            }
        }
    }
}
