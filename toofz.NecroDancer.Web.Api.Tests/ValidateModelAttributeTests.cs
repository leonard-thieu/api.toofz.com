using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Web.Api.Tests
{
    class ValidateModelAttributeTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var validate = new ValidateModelAttribute();

                // Assert
                Assert.IsInstanceOfType(validate, typeof(ValidateModelAttribute));
            }
        }

        [TestClass]
        public class OnActionExecutingMethod
        {
            [TestMethod]
            public void ModelStateIsValid_DoesNotSetResponse()
            {
                // Arrange
                var validate = new ValidateModelAttribute();
                var actionContext = new HttpActionContext();

                // Act
                validate.OnActionExecuting(actionContext);

                // Assert
                Assert.IsNull(actionContext.Response);
            }

            [TestMethod]
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
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
