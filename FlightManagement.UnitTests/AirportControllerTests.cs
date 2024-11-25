using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using Moq;
using System.Text;
using System.Threading.Tasks;
using FlightManagement.ASPnet.Controllers;
using FlightManagement.DTO.Airport;
using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.ASPnet.Models;
using FlightManagement.DAL.models;
using FlightManagement.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using FlightManagement.models;



namespace FlightManagement.UnitTests
{
    public class AirportControllerTests
    {
        private readonly AirportController _controller;
        private readonly Mock<IAirportService> _mockAirportService;
        private readonly Mock<IMapper> _mockMapper;

        public AirportControllerTests()
        {
            _mockAirportService = new Mock<IAirportService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new AirportController(_mockAirportService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithCorrectViewModel()
        {
            // Arrange
            var airports = new List<Airport> { new Airport { AirportID = 1, Name = "Airport1", Location = "Location1" } };
            var airportsDto = new List<AirportsDto> { new AirportsDto { AirportID = 1, Name = "Airport1", Location = "Location1" } };
            _mockAirportService.Setup(service => service.GetAll()).ReturnsAsync(airports);
            _mockMapper.Setup(m => m.Map<IEnumerable<AirportsDto>>(airports)).Returns(airportsDto);

            // Act
            var result = await _controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AirportViewModel>(viewResult.Model);
            Assert.Single(model.Airports);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange
            var airportForCreation = new AirportsForCreationDto { Name = "New Airport", Location = "New Location" };
            _controller.ModelState.Clear(); // Clear any model state errors

            // Act
            var result = await _controller.Create(airportForCreation);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Create_Post_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var airportForCreation = new AirportsForCreationDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(airportForCreation);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(airportForCreation, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewResult_WithAirportForUpdateDto()
        {
            // Arrange
            var airport = new Airport { AirportID = 1, Name = "Airport1", Location = "Location1" };
            _mockAirportService.Setup(s => s.GetById(1)).ReturnsAsync(airport);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AirportsForUpdateDto>(viewResult.Model);
            Assert.Equal(airport.AirportID, model.AirportID);
        }

        [Fact]
        public async Task ConfirmDelete_ReturnsViewResult_WithAirportDto()
        {
            // Arrange
            var airport = new Airport { AirportID = 1, Name = "Airport1", Location = "Location1" };
            _mockAirportService.Setup(s => s.GetById(1)).ReturnsAsync(airport);

            // Act
            var result = await _controller.ConfirmDelete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AirportsDto>(viewResult.Model);
            Assert.Equal(airport.AirportID, model.AirportID);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToAction_WhenSuccessful()
        {
            // Arrange
            _mockAirportService.Setup(s => s.Delete(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
        [Fact]
        public async Task Index_ReturnsCorrectCountOfAirports()
        {
            // Arrange
            var airports = new List<Airport>
    {
        new Airport { AirportID = 1, Name = "Airport1", Location = "Location1" },
        new Airport { AirportID = 2, Name = "Airport2", Location = "Location2" }
    };
            var airportsDto = new List<AirportsDto>
    {
        new AirportsDto { AirportID = 1, Name = "Airport1", Location = "Location1" },
        new AirportsDto { AirportID = 2, Name = "Airport2", Location = "Location2" }
    };
            _mockAirportService.Setup(service => service.GetAll()).ReturnsAsync(airports);
            _mockMapper.Setup(m => m.Map<IEnumerable<AirportsDto>>(airports)).Returns(airportsDto);

            // Act
            var result = await _controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AirportViewModel>(viewResult.Model);
            Assert.Equal(2, model.Airports.Count());
        }


        [Fact]
        public async Task Index_ReturnsZeroAirports_WhenNoneExist()
        {
            // Arrange
            var airports = new List<Airport>();
            _mockAirportService.Setup(service => service.GetAll()).ReturnsAsync(airports);

            // Act
            var result = await _controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AirportViewModel>(viewResult.Model);
            Assert.Empty(model.Airports);
        }
    }
}