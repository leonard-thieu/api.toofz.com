using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using toofz.NecroDancer.Web.Api.Infrastructure;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class ValidateModelAttributeTests
    {
        public class Constructor
        {
            [DisplayFact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var validate = new ValidateModelAttribute();

                // Assert
                Assert.IsAssignableFrom<ValidateModelAttribute>(validate);
            }
        }

        public class OnActionExecutingMethod
        {
            [DisplayFact]
            public void ModelStateIsValid_DoesNotSetResponse()
            {
                // Arrange
                var validate = new ValidateModelAttribute();
                var actionContext = new HttpActionContext();

                // Act
                validate.OnActionExecuting(actionContext);

                // Assert
                Assert.Null(actionContext.Response);
            }

            [DisplayFact]
            public void ModelStateIsInvalid_SetsErrorResponse()
            {
                // Arrange
                var validate = new ValidateModelAttribute();
                var request = new HttpRequestMessage();
                var actionContext = ContextUtil.GetHttpActionContext(request);
                actionContext.ModelState.AddModelError("myKey", "myError");

                // Act
                validate.OnActionExecuting(actionContext);

                // Assert
                var response = actionContext.Response;
                Assert.NotNull(response);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
