using FlightManagement.ASPnet.Models;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Stops;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteApiController : ControllerBase
    {
        private readonly ILogger<RouteApiController> _logger;
        private readonly IRepositoryManager _repositoryManager;

        public RouteApiController(ILogger<RouteApiController> logger, IRepositoryManager repositoryManager)
        {
            _logger = logger;
            _repositoryManager = repositoryManager;
        }

        // Получение списка всех маршрутов
        [HttpGet]
        public async Task<IActionResult> GetRoutes(string flightNumber, DateTime? departureDate)
        {
            var routes = await _repositoryManager.RoutesRepository.GetAllEntities(false)
                .Select(r => new
                {
                    r.RouteID,
                    r.DepartureTime,
                    r.Date,
                    FlightNumber = r.Flight.FlightNumber, // Предполагается, что FlightNumber доступен через навигационное свойство
                }).ToListAsync();

            // Фильтрация
            if (!string.IsNullOrEmpty(flightNumber))
            {
                routes = routes.Where(r => r.FlightNumber.Contains(flightNumber, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (departureDate.HasValue)
            {
                routes = routes.Where(r => r.Date.Date == departureDate.Value.Date).ToList();
            }

            return Ok(routes);
        }

        // Получение маршрута по ID
        [HttpGet("{routeID}")]
        public async Task<IActionResult> GetRouteById(int routeID)
        {
            var route = await _repositoryManager.RoutesRepository.GetById(routeID, trackChanges: false);
            if (route == null)
            {
                return NotFound();
            }

            var routeDto = new RouteDetailsDto
            {
                RouteID = route.RouteID,
                DepartureTime = route.DepartureTime,
                Date = route.Date,
                Stops = route.Stops.Select(s => new StopsDto
                {
                    StopID = s.StopID,
                    ArrivalTime = s.ArrivalTime,
                    DepartureTime = s.DepartureTime,
                    AirportID = s.AirportID,
                    StatusID = s.StatusID,
                    AirportName = s.Airport.Name,
                    AirportLocation = s.Airport.Location,
                    StatusName = s.Status.StatusName
                }).OrderBy(s => s.DepartureTime).ToList()
            };

            return Ok(routeDto);
        }

        // Создание маршрута
        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] RoutesForCreationDto routeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var route = new models.Route
            {
                FlightID = routeDto.FlightID,
                DepartureTime = routeDto.DepartureTime,
                Date = routeDto.Date,
                Stops = routeDto.Stops.Select(s => new Stop
                {
                    AirportID = s.AirportID,
                    ArrivalTime = s.ArrivalTime,
                    DepartureTime = s.DepartureTime,
                    StatusID = s.StatusID
                }).ToList()
            };

            await _repositoryManager.RoutesRepository.Create(route);
            await _repositoryManager.SaveAsync();

            return CreatedAtAction(nameof(GetRouteById), new { routeID = route.RouteID }, route);
        }

        // Обновление маршрута
        [HttpPut("{routeID}")]
        public async Task<IActionResult> UpdateRoute(int routeID, [FromBody] RoutesForUpdateDto routeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var route = await _repositoryManager.RoutesRepository.GetById(routeID, trackChanges: true);
            if (route == null)
            {
                return NotFound();
            }

            route.FlightID = routeDto.FlightID;
            route.DepartureTime = routeDto.DepartureTime;
            route.Date = routeDto.Date;

            // Обновление остановок
            foreach (var stopDto in routeDto.Stops)
            {
                var existingStop = route.Stops.FirstOrDefault(s => s.StopID == stopDto.StopID);
                if (existingStop != null)
                {
                    existingStop.AirportID = stopDto.AirportID;
                    existingStop.ArrivalTime = stopDto.ArrivalTime;
                    existingStop.DepartureTime = stopDto.DepartureTime;
                    existingStop.StatusID = stopDto.StatusID;
                }
                else
                {
                    route.Stops.Add(new Stop
                    {
                        AirportID = stopDto.AirportID,
                        ArrivalTime = stopDto.ArrivalTime,
                        DepartureTime = stopDto.DepartureTime,
                        StatusID = stopDto.StatusID
                    });
                }
            }

            await _repositoryManager.SaveAsync();

            return NoContent(); // 204 No Content
        }

        // Удаление маршрута
        [HttpDelete("{routeID}")]
        public async Task<IActionResult> DeleteRoute(int routeID)
        {
            var route = await _repositoryManager.RoutesRepository.GetById(routeID, trackChanges: false);
            if (route == null)
            {
                return NotFound();
            }

            await _repositoryManager.RoutesRepository.Delete(route);
            await _repositoryManager.SaveAsync();

            return NoContent(); // 204 No Content
        }
    }
}