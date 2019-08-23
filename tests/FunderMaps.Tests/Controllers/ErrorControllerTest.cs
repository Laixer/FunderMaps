using FunderMaps.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static FunderMaps.Controllers.ErrorController;

namespace FunderMaps.Tests.Controllers
{
    public class ErrorControllerTest
    {
        [Fact]
        public void Error_controller_returns_error500()
        {
            // Arrange
            using (var controller = new ErrorController())
            {
                int errorCode = StatusCodes.Status500InternalServerError;

                // act
                var result = controller.Error() as ObjectResult;

                //assert
                Assert.Equal(result.StatusCode, errorCode);
            }
        }

        [Fact]
        public void Error_controller_error500_title_matches_error_title()
        {
            // Arrange
            using (var controller = new ErrorController())
            {
                string errorTitle = "An error has occured on the remote side";
                int errorCode = StatusCodes.Status500InternalServerError;

                // Act
                var result = controller.Error() as ObjectResult;
                var model = (ServerErrorOutoutModel)result.Value;

                // Assert
                Assert.Equal(model.Title, errorTitle);
                Assert.Equal(model.Status, errorCode);
            }
        }
    }
}
