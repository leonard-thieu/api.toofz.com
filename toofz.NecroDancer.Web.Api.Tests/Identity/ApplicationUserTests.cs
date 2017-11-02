﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Moq;
using toofz.NecroDancer.Web.Api.Identity;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Identity
{
    public class ApplicationUserTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var user = new ApplicationUser();

                // Assert
                Assert.IsAssignableFrom<ApplicationUser>(user);
            }
        }

        public class GenerateUserIdentityAsyncMethod
        {
            [Fact]
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
                Assert.IsAssignableFrom<ClaimsIdentity>(claimsIdentity);
            }
        }
    }
}
