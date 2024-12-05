using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DTO.Airport;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.ASPnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirportsApiController : ControllerBase
    {
        private readonly IAirportService _airportService;
        private readonly IMapper _mapper;

        public AirportsApiController(IAirportService airportService, IMapper mapper)
        {
            _airportService = airportService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirportsDto>>> GetAllAirports(string searchTerm)
        {
            var airports = await _airportService.GetAll();
            var airportsDto = _mapper.Map<IEnumerable<AirportsDto>>(airports);

            // Поиск
            if (!string.IsNullOrEmpty(searchTerm))
            {
                airportsDto = airportsDto.Where(a => a.Name.Contains(searchTerm) || a.Location.Contains(searchTerm));
            }

            return Ok(airportsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirportsDto>> GetAirportById(int id)
        {
            var airport = await _airportService.GetById(id);
            if (airport == null) return NotFound();

            var airportDto = _mapper.Map<AirportsDto>(airport);
            return Ok(airportDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAirport([FromBody] AirportsForCreationDto airportForCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _airportService.Create(airportForCreation);
            return CreatedAtAction(nameof(GetAirportById), new { id = airportForCreation.AirportID }, airportForCreation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAirport(int id, [FromBody] AirportsForUpdateDto airportForUpdate)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _airportService.Update(airportForUpdate);
            if (!result) return NotFound();

            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            var result = await _airportService.Delete(id);
            if (!result) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
