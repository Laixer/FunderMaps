using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using FunderMaps.Controllers.Webservice;
using static FunderMaps.Controllers.Webservice.VersionController;

namespace FunderMaps.Tests
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
