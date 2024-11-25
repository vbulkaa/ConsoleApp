using FlightManagement.ASPnet.Models;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.Controllers;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Stops;
using FlightManagement.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.UnitTests
{
    public class RouteControllerTests
    {
        private readonly Mock<IFlightService> _mockFlightService;
        private readonly RouteController _controller;
        private readonly Mock<ILogger<RouteController>> _mockLogger;
        private readonly Mock<IRouteService> _mockRouteService;
        private readonly Mock<IStatusService> _mockStatusService;
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<IAirportService> _mockAirportService;

        public RouteControllerTests()
        {
            _mockLogger = new Mock<ILogger<RouteController>>();
            _mockRouteService = new Mock<IRouteService>();
            _mockFlightService = new Mock<IFlightService>();
            _mockStatusService = new Mock<IStatusService>();
            _mockAirportService = new Mock<IAirportService>();
            _mockRepositoryManager = new Mock<IRepositoryManager>();

            _controller = new RouteController(
                _mockLogger.Object,
                _mockRouteService.Object,
                _mockStatusService.Object,
                _mockFlightService.Object,
                _mockRepositoryManager.Object,
                _mockAirportService.Object
            );
        }

        [Fact]
        public async Task Create_Get_ReturnsViewWithData()
        {
            // Arrange
            _mockRepositoryManager.Setup(repo => repo.FlightsRepository.GetAll(false))
                .ReturnsAsync(new List<Flight> { new Flight { FlightID = 1, FlightNumber = "FL123" } });
            _mockRepositoryManager.Setup(repo => repo.AirportsRepository.GetAll(false))
                .ReturnsAsync(new List<Airport> { new Airport { AirportID = 1, Name = "Airport A" } });
            _mockRepositoryManager.Setup(repo => repo.StatusesRepository.GetAll(false))
                .ReturnsAsync(new List<Status> { new Status { StatusID = 1, StatusName = "Scheduled" } });

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Flights"]);
            Assert.NotNull(viewResult.ViewData["Airports"]);
            Assert.NotNull(viewResult.ViewData["Statuses"]);
        }

        [Fact]
        public async Task Create_Post_ValidRoute_RedirectsToIndex()
        {
            // Arrange
            var routeDto = new RoutesForCreationDto
            {
                FlightID = 1,
                DepartureTime = TimeSpan.FromHours(1),
                Date = DateTime.Now.AddDays(1),
                Stops = new List<StopsForCreationDto>
                {
                    new StopsForCreationDto
                    {
                        AirportID = 1,
                        ArrivalTime = TimeSpan.FromHours(1),
                        DepartureTime = TimeSpan.FromHours(2),
                        StatusID = 1
                    }
                }
            };

            _mockRepositoryManager.Setup(repo => repo.RoutesRepository.Create(It.IsAny<Route>())).Returns(Task.CompletedTask);
            _mockRepositoryManager.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(routeDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

       
       
        [Fact]
        public async Task ConfirmDelete_ExistingRoute_ReturnsViewResult()
        {
            // Arrange
            var route = new Route { RouteID = 1, FlightID = 1 };
            _mockRepositoryManager.Setup(repo => repo.RoutesRepository.GetById(1, false)).ReturnsAsync(route);

            // Act
            var result = await _controller.ConfirmDelete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(route.RouteID, ((Route)viewResult.Model).RouteID);
        }

        [Fact]
        public async Task Create_Get_ReturnsCorrectCountOfFlights()
        {
            // Arrange
            var flights = new List<Flight>
    {
        new Flight { FlightID = 1, FlightNumber = "FL123" },
        new Flight { FlightID = 2, FlightNumber = "FL456" }
    };
            _mockRepositoryManager.Setup(repo => repo.FlightsRepository.GetAll(false))
                .ReturnsAsync(flights);
            _mockRepositoryManager.Setup(repo => repo.AirportsRepository.GetAll(false))
                .ReturnsAsync(new List<Airport> { new Airport { AirportID = 1, Name = "Airport A" } });
            _mockRepositoryManager.Setup(repo => repo.StatusesRepository.GetAll(false))
                .ReturnsAsync(new List<Status> { new Status { StatusID = 1, StatusName = "Scheduled" } });

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var flightsList = Assert.IsAssignableFrom<List<Flight>>(viewResult.ViewData["Flights"]);
            Assert.Equal(2, flightsList.Count);
        }

        [Fact]
        public async Task Create_Get_ReturnsCorrectCountOfAirports()
        {
            // Arrange
            var airports = new List<Airport>
    {
        new Airport { AirportID = 1, Name = "Airport A" },
        new Airport { AirportID = 2, Name = "Airport B" }
    };
            _mockRepositoryManager.Setup(repo => repo.FlightsRepository.GetAll(false))
                .ReturnsAsync(new List<Flight> { new Flight { FlightID = 1, FlightNumber = "FL123" } });
            _mockRepositoryManager.Setup(repo => repo.AirportsRepository.GetAll(false))
                .ReturnsAsync(airports);
            _mockRepositoryManager.Setup(repo => repo.StatusesRepository.GetAll(false))
                .ReturnsAsync(new List<Status> { new Status { StatusID = 1, StatusName = "Scheduled" } });

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var airportsList = Assert.IsAssignableFrom<List<Airport>>(viewResult.ViewData["Airports"]);
            Assert.Equal(2, airportsList.Count);
        }

        [Fact]
        public async Task Create_Get_ReturnsCorrectCountOfStatuses()
        {
            // Arrange
            var statuses = new List<Status>
    {
        new Status { StatusID = 1, StatusName = "Scheduled" },
        new Status { StatusID = 2, StatusName = "Delayed" }
    };
            _mockRepositoryManager.Setup(repo => repo.FlightsRepository.GetAll(false))
                .ReturnsAsync(new List<Flight> { new Flight { FlightID = 1, FlightNumber = "FL123" } });
            _mockRepositoryManager.Setup(repo => repo.AirportsRepository.GetAll(false))
                .ReturnsAsync(new List<Airport> { new Airport { AirportID = 1, Name = "Airport A" } });
            _mockRepositoryManager.Setup(repo => repo.StatusesRepository.GetAll(false))
                .ReturnsAsync(statuses);

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var statusesList = Assert.IsAssignableFrom<List<Status>>(viewResult.ViewData["Statuses"]);
            Assert.Equal(2, statusesList.Count);
        }

    }


}
