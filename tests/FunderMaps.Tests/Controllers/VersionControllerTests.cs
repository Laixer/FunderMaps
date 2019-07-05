using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using FunderMaps.Controllers.Api;
using static FunderMaps.Controllers.Api.VersionController;

namespace FunderMaps.Tests.Controllers
{
    public class VersionControllerTests
    {
        [Fact]
        public void Get_ReturnsVersionOutputModel()
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
