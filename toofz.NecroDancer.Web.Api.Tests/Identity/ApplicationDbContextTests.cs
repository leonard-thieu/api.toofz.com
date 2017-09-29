using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Web.Api.Identity;

namespace toofz.NecroDancer.Web.Api.Tests.Identity
{
    class ApplicationDbContextTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var db = new ApplicationDbContext();

                // Assert
                Assert.IsInstanceOfType(db, typeof(ApplicationDbContext));
            }
        }

        [TestClass]
        public class Constructor_String
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var connectionString = "Data Source=localhost;Integrated Security=SSPI";

                // Act
                var db = new ApplicationDbContext(connectionString);

                // Assert
                Assert.IsInstanceOfType(db, typeof(ApplicationDbContext));
            }
        }
    }
}
