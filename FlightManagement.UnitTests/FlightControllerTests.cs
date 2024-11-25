using FlightManagement.ASPnet.Controllers;
using FlightManagement.ASPnet.Models;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DAL.models.enums;
using FlightManagement.DTO.Flights;
using FlightManagement.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.UnitTests
{
    public class FlightControllerTests
    {
        private readonly FlightController _controller;
        private readonly Mock<IRepositoryManager> _mockRepository;
        private readonly Mock<ILogger<FlightController>> _mockLogger;

        public FlightControllerTests()
        {
            _mockRepository = new Mock<IRepositoryManager>();
            _mockLogger = new Mock<ILogger<FlightController>>();
            _controller = new FlightController(_mockRepository.Object, _mockLogger.Object);
        }

      

        [Fact]
        public async Task Edit_ExistingFlight_ReturnsViewResult()
        {
            // Arrange
            var flight = new Flight { FlightID = 1, FlightNumber = "FL123", AircraftType = AircraftType.AirbusA320.ToString(), TicketPrice = 200.00m };
            _mockRepository.Setup(repo => repo.FlightsRepository.GetById(1, false)).ReturnsAsync(flight);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<FlightsForUpdateDto>(viewResult.Model);
            Assert.Equal("FL123", model.FlightNumber);
        }

        [Fact]
        public async Task Delete_ExistingFlight_ReturnsViewResult()
        {
            // Arrange
            var flight = new Flight { FlightID = 1, FlightNumber = "FL123" };
            _mockRepository.Setup(repo => repo.FlightsRepository.GetById(1, false)).ReturnsAsync(flight);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Flight>(viewResult.Model);
            Assert.Equal("FL123", model.FlightNumber);
        }

        [Fact]
        public async Task Edit_NonExistingFlight_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.FlightsRepository.GetById(1, false)).ReturnsAsync((Flight)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistingFlight_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.FlightsRepository.GetById(1, false)).ReturnsAsync((Flight)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

       

       
    }
}
