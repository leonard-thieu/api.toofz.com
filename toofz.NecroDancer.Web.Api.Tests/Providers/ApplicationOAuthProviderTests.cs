using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OAuth.Messages;
using Moq;
using toofz.NecroDancer.Web.Api.Identity;
using toofz.NecroDancer.Web.Api.Providers;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Providers
{
    public class ApplicationOAuthProviderTests
    {
        public class Constructor
        {
            [Fact]
            public void PublicClientIdIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string publicClientId = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new ApplicationOAuthProvider(publicClientId);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var publicClientId = "myPublicClientId";

                // Act
                var provider = new ApplicationOAuthProvider(publicClientId);

                // Assert
                Assert.IsAssignableFrom<ApplicationOAuthProvider>(provider);
            }
        }

        public class GrantResourceOwnerCredentialsMethod
        {
            [Fact]
            public async Task UserNotFound_SetsError()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var userManager = Mock.Of<ApplicationUserManagerAdapter>();
                var mockOwinContext = new Mock<IOwinContext>();
                mockOwinContext.Setup(c => c.Get<ApplicationUserManager>(It.IsAny<string>())).Returns(userManager);
                var owinContext = mockOwinContext.Object;
                var options = new OAuthAuthorizationServerOptions();
                var clientId = "myPublicClientId";
                var userName = "myUserName";
                var password = "myPassword";
                var scope = new List<string>();
                var context = new OAuthGrantResourceOwnerCredentialsContext(owinContext, options, clientId, userName, password, scope);

                // Act
                await provider.GrantResourceOwnerCredentials(context);

                // Assert
                Assert.Equal("invalid_grant", context.Error);
            }

            [Fact]
            public async Task Validates()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var userName = "myUserName";
                var password = "myPassword";
                var mockUserManager = new Mock<ApplicationUserManagerAdapter>();
                mockUserManager.Setup(m => m.FindAsync(userName, password)).Returns(Task.FromResult(new ApplicationUser()));
                var userManager = mockUserManager.Object;
                var mockOwinRequest = new Mock<IOwinRequest>();
                var owinRequest = mockOwinRequest.Object;
                var mockAuthenticationManager = new Mock<IAuthenticationManager>();
                var authenticationManager = mockAuthenticationManager.Object;
                var mockOwinContext = new Mock<IOwinContext>();
                mockOwinContext.Setup(c => c.Get<ApplicationUserManager>(It.IsAny<string>())).Returns(userManager);
                mockOwinContext.SetupGet(c => c.Request).Returns(owinRequest);
                mockOwinContext.SetupGet(c => c.Authentication).Returns(authenticationManager);
                var owinContext = mockOwinContext.Object;
                mockOwinRequest.SetupGet(r => r.Context).Returns(owinContext);
                var options = new OAuthAuthorizationServerOptions();
                var clientId = "myPublicClientId";
                var scope = new List<string>();
                var context = new OAuthGrantResourceOwnerCredentialsContext(owinContext, options, clientId, userName, password, scope);

                // Act
                await provider.GrantResourceOwnerCredentials(context);

                // Assert
                Assert.True(context.IsValidated);
                Assert.False(context.HasError);
            }

            [Fact]
            public async Task SignsIn()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var userName = "myUserName";
                var password = "myPassword";
                var mockUserManager = new Mock<ApplicationUserManagerAdapter>();
                mockUserManager.Setup(m => m.FindAsync(userName, password)).Returns(Task.FromResult(new ApplicationUser()));
                var userManager = mockUserManager.Object;
                var mockOwinRequest = new Mock<IOwinRequest>();
                var owinRequest = mockOwinRequest.Object;
                var mockAuthenticationManager = new Mock<IAuthenticationManager>();
                var authenticationManager = mockAuthenticationManager.Object;
                var mockOwinContext = new Mock<IOwinContext>();
                mockOwinContext.Setup(c => c.Get<ApplicationUserManager>(It.IsAny<string>())).Returns(userManager);
                mockOwinContext.SetupGet(c => c.Request).Returns(owinRequest);
                mockOwinContext.SetupGet(c => c.Authentication).Returns(authenticationManager);
                var owinContext = mockOwinContext.Object;
                mockOwinRequest.SetupGet(r => r.Context).Returns(owinContext);
                var options = new OAuthAuthorizationServerOptions();
                var clientId = "myPublicClientId";
                var scope = new List<string>();
                var context = new OAuthGrantResourceOwnerCredentialsContext(owinContext, options, clientId, userName, password, scope);

                // Act
                await provider.GrantResourceOwnerCredentials(context);

                // Assert
                mockAuthenticationManager.Verify(a => a.SignIn(It.IsAny<ClaimsIdentity>()), Times.Once);
            }

            public class ApplicationUserManagerAdapter : ApplicationUserManager
            {
                public ApplicationUserManagerAdapter() : base(Mock.Of<IUserStore<ApplicationUser>>()) { }
            }
        }

        public class TokenEndpointMethod
        {
            [Fact]
            public async Task AddsAdditionalResponseParameters()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var owinContext = Mock.Of<IOwinContext>();
                var options = new OAuthAuthorizationServerOptions();
                var identity = new ClaimsIdentity();
                var properties = new AuthenticationProperties();
                properties.Dictionary.Add("myKey", "myValue");
                var ticket = new AuthenticationTicket(identity, properties);
                var parameters = Mock.Of<IReadableStringCollection>();
                var tokenEndpointRequest = new TokenEndpointRequest(parameters);
                var context = new OAuthTokenEndpointContext(owinContext, options, ticket, tokenEndpointRequest);

                // Act
                await provider.TokenEndpoint(context);

                // Assert
                Assert.True(context.AdditionalResponseParameters.ContainsKey("myKey"));
                var value = context.AdditionalResponseParameters["myKey"];
                Assert.Equal("myValue", value);
            }
        }

        public class ValidateClientAuthenticationMethod
        {
            [Fact]
            public async Task ClientIdIsNull_Validates()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var owinContext = Mock.Of<IOwinContext>();
                var options = new OAuthAuthorizationServerOptions();
                var parameters = Mock.Of<IReadableStringCollection>();
                var context = new OAuthValidateClientAuthenticationContext(owinContext, options, parameters);

                // Act
                await provider.ValidateClientAuthentication(context);

                // Assert
                Assert.True(context.IsValidated);
                Assert.False(context.HasError);
            }

            [Fact]
            public async Task ClientIdIsNotNull_DoesNotValidate()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var owinContext = Mock.Of<IOwinContext>();
                var options = new OAuthAuthorizationServerOptions();
                var parameters = Mock.Of<IReadableStringCollection>();
                var clientId = "myClientId";
                var context = new OAuthValidateClientAuthenticationContextAdapter(owinContext, options, parameters, clientId);

                // Act
                await provider.ValidateClientAuthentication(context);

                // Assert
                Assert.False(context.IsValidated);
            }

            private sealed class OAuthValidateClientAuthenticationContextAdapter : OAuthValidateClientAuthenticationContext
            {
                public OAuthValidateClientAuthenticationContextAdapter(IOwinContext context, OAuthAuthorizationServerOptions options, IReadableStringCollection parameters, string clientId) :
                    base(context, options, parameters)
                {
                    ClientId = clientId;
                }
            }
        }

        public class ValidateClientRedirectUriMethod
        {
            [Fact]
            public async Task ClientIdAndRedirectUriMatch_Validates()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var mockOwinRequest = new Mock<IOwinRequest>();
                mockOwinRequest.SetupGet(r => r.Uri).Returns(new Uri("http://example.org/"));
                var owinRequest = mockOwinRequest.Object;
                var mockOwinContext = new Mock<IOwinContext>();
                mockOwinContext.SetupGet(c => c.Request).Returns(owinRequest);
                var owinContext = mockOwinContext.Object;
                var options = new OAuthAuthorizationServerOptions();
                var clientId = "myPublicClientId";
                var redirectUri = "http://example.org/";
                var context = new OAuthValidateClientRedirectUriContext(owinContext, options, clientId, redirectUri);

                // Act
                await provider.ValidateClientRedirectUri(context);

                // Assert
                Assert.True(context.IsValidated);
                Assert.False(context.HasError);
            }

            [Fact]
            public async Task ClientIdMatchesAndRedirectUriDoesNotMatch_DoesNotValidate()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var mockOwinRequest = new Mock<IOwinRequest>();
                mockOwinRequest.SetupGet(r => r.Uri).Returns(new Uri("http://example.com/"));
                var owinRequest = mockOwinRequest.Object;
                var mockOwinContext = new Mock<IOwinContext>();
                mockOwinContext.SetupGet(c => c.Request).Returns(owinRequest);
                var owinContext = mockOwinContext.Object;
                var options = new OAuthAuthorizationServerOptions();
                var clientId = "myPublicClientId";
                var redirectUri = "http://example.org/";
                var context = new OAuthValidateClientRedirectUriContext(owinContext, options, clientId, redirectUri);

                // Act
                await provider.ValidateClientRedirectUri(context);

                // Assert
                Assert.False(context.IsValidated);
            }

            [Fact]
            public async Task ClientIdDoesNotMatch_DoesNotValidate()
            {
                // Arrange
                var publicClientId = "myPublicClientId";
                var provider = new ApplicationOAuthProvider(publicClientId);
                var mockOwinRequest = new Mock<IOwinRequest>();
                mockOwinRequest.SetupGet(r => r.Uri).Returns(new Uri("http://example.com/"));
                var owinRequest = mockOwinRequest.Object;
                var mockOwinContext = new Mock<IOwinContext>();
                mockOwinContext.SetupGet(c => c.Request).Returns(owinRequest);
                var owinContext = mockOwinContext.Object;
                var options = new OAuthAuthorizationServerOptions();
                var clientId = "myInvalidPublicClientId";
                var redirectUri = "http://example.org/";
                var context = new OAuthValidateClientRedirectUriContext(owinContext, options, clientId, redirectUri);

                // Act
                await provider.ValidateClientRedirectUri(context);

                // Assert
                Assert.False(context.IsValidated);
            }
        }

        public class CreatePropertiesMethod
        {
            [Fact]
            public void ReturnsAuthenticationProperties()
            {
                // Arrange
                var userName = "myUserName";

                // Act
                var properties = ApplicationOAuthProvider.CreateProperties(userName);

                // Assert
                Assert.True(properties.Dictionary.ContainsKey("userName"));
                Assert.Equal(userName, properties.Dictionary["userName"]);
            }
        }
    }
}
