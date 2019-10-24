using FunderMaps.Controllers.Api;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FunderMaps.Tests.Controllers
{
    public class VersionControllerTests
    {
        [Fact]
        public void GetReturnsVersionOutputModel()
        {
            // Arrange
            var controller = new VersionController();

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<VersionOutputModel>(okResult.Value);
            Assert.NotNull(returnValue.Name);
            Assert.NotNull(returnValue.VersionString);
            Assert.Equal(returnValue.VersionString, returnValue.Version.ToString());
        }
    }
}
