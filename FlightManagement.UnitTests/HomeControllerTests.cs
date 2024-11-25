using Moq;
using FlightManagement.ASPnet.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlightManagement.ASPnet.Models;
using Xunit;

namespace FlightManagement.UnitTests
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;
        private readonly Mock<ILogger<HomeController>> _mockLogger;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Home_ReturnsViewResult()
        {
            // Act
            var result = _controller.Home();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Tables_ReturnsViewResult()
        {
            // Act
            var result = _controller.Tables();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void ActionMethods_Count()
        {
            // Arrange
            var expectedActionCount = 4; // Update this if you add more actions

            // Act
            var actions = typeof(HomeController).GetMethods()
                .Where(m => m.ReturnType == typeof(ViewResult) && !m.IsSpecialName)
                .ToList();

            // Assert
            Assert.Equal(expectedActionCount, actions.Count);
        }
    }
}