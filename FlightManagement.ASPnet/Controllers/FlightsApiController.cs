using AutoMapper;
using FlightManagement.ASPnet.Models;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Flights;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.ASPnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsApiController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public FlightsApiController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightSchedule>>> GetFlights(string airportName = null, string routeID = null, TimeSpan? departureTime = null)
        {
            var departureSchedules = await GetFlightSchedulesByStatus("вылет");
            var arrivalSchedules = await GetFlightSchedulesByStatus("прилет");

            var flightSchedules = departureSchedules.Concat(arrivalSchedules)
                                                     .Distinct(new FlightScheduleComparer())
                                                     .ToList();

            // Поиск
            if (!string.IsNullOrEmpty(airportName))
            {
                flightSchedules = flightSchedules.Where(fs =>
                    fs.DepartureAirportName.Contains(airportName) ||
                    fs.ArrivalAirportName.Contains(airportName)).ToList();
            }

            if (!string.IsNullOrEmpty(routeID))
            {
                flightSchedules = flightSchedules.Where(fs =>
                    fs.RouteID.ToString().Equals(routeID)).ToList();
            }

            if (departureTime.HasValue)
            {
                flightSchedules = flightSchedules.Where(fs =>
                    fs.DepartureTime >= departureTime.Value).ToList();
            }

            return Ok(flightSchedules.OrderBy(fs => fs.DepartureTime).ToList());
        }

        private async Task<IEnumerable<FlightSchedule>> GetFlightSchedulesByStatus(string status)
        {
            var flightsQuery = _repository.FlightsRepository.GetAllEntities(false)
                .Join(_repository.RoutesRepository.GetAllEntities(false),
                    flight => flight.FlightID,
                    route => route.FlightID,
                    (flight, route) => new { flight, route })
                .Join(_repository.StopsRepository.GetAllEntities(false),
                    fr => fr.route.RouteID,
                    stop => stop.RouteID,
                    (fr, stop) => new { fr.flight, fr.route, stop })
                .Join(_repository.AirportsRepository.GetAllEntities(false),
                    frs => frs.stop.AirportID,
                    airport => airport.AirportID,
                    (frs, airport) => new
                    {
                        frs.flight,
                        frs.route,
                        frs.stop,
                        Airport = airport
                    });

            // Группируем результаты по маршруту
            var groupedFlights = flightsQuery
                .GroupBy(g => new { g.route.RouteID, g.flight.FlightNumber, g.flight.AircraftType, g.flight.TicketPrice })
                .Select(g => new FlightSchedule
                {
                    RouteID = g.Key.RouteID,
                    FlightNumber = g.Key.FlightNumber,
                    AircraftType = g.Key.AircraftType,
                    TicketPrice = g.Key.TicketPrice,
                    DepartureAirportName = g.FirstOrDefault(f => f.stop.Status.StatusName == "вылет").Airport.Name,
                    DepartureAirportLocation = g.FirstOrDefault(f => f.stop.Status.StatusName == "вылет").Airport.Location,
                    DepartureTime = g.FirstOrDefault(f => f.stop.Status.StatusName == "вылет").stop.DepartureTime,
                    Date = g.FirstOrDefault(f => f.stop.Status.StatusName == "вылет").route.Date,
                    DepartureStatusName = g.FirstOrDefault(f => f.stop.Status.StatusName == "вылет").stop.Status.StatusName,
                    ArrivalTime = g.FirstOrDefault(f => f.stop.Status.StatusName == "прилет").stop.ArrivalTime,
                    ArrivalAirportName = g.FirstOrDefault(f => f.stop.Status.StatusName == "прилет").Airport.Name,
                    ArrivalAirportLocation = g.FirstOrDefault(f => f.stop.Status.StatusName == "прилет").Airport.Location,
                    ArrivalStatusName = g.FirstOrDefault(f => f.stop.Status.StatusName == "прилет").stop.Status.StatusName
                });

            return await groupedFlights.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlightById(int id)
        {
            var flight = await _repository.FlightsRepository.GetById(id, trackChanges: false);
            if (flight == null) return NotFound();

            var flightDto = _mapper.Map<Flight>(flight);
            return Ok(flightDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFlight([FromBody] FlightsForCreationDto flightDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var flight = new Flight
            {
                FlightNumber = flightDto.FlightNumber,
                AircraftType = flightDto.AircraftType,
                TicketPrice = flightDto.TicketPrice
            };

            await _repository.FlightsRepository.Create(flight);
            await _repository.SaveAsync();

            return CreatedAtAction(nameof(GetFlightById), new { id = flight.FlightID }, flight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] FlightsForUpdateDto flightDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var flight = await _repository.FlightsRepository.GetById(id, trackChanges: false);
            if (flight == null) return NotFound();

            flight.FlightNumber = flightDto.FlightNumber;
            flight.AircraftType = flightDto.AircraftType;
            flight.TicketPrice = flightDto.TicketPrice;

            _repository.FlightsRepository.Update(flight);
            await _repository.SaveAsync();

            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _repository.FlightsRepository.GetById(id, trackChanges: false);
            if (flight == null) return NotFound();

            await _repository.FlightsRepository.Delete(flight);
            await _repository.SaveAsync();

            return NoContent(); // 204 No Content
        }
    }
}
