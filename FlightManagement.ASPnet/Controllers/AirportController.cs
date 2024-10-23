using FlightManagement.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Linq;
using System.Threading.Tasks;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DTO.Airport;
using FlightManagement.ASPnet.Models;
using AutoMapper;



namespace FlightManagement.ASPnet.Controllers
{
    public class AirportController : Controller
    {
        private readonly IAirportService _airportService;
        private readonly IMapper _mapper;

        public AirportController(IAirportService airportService, IMapper mapper)
        {
            _airportService = airportService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchTerm, string sortOrder)
        {
            var airports = await _airportService.GetAll();
            var airportsDto = _mapper.Map<IEnumerable<AirportsDto>>(airports);

            // Поиск
            if (!string.IsNullOrEmpty(searchTerm))
            {
                airportsDto = airportsDto.Where(a => a.Name.Contains(searchTerm) || a.Location.Contains(searchTerm));
            }

            // Сортировка
            switch (sortOrder)
            {
                case "name_desc":
                    airportsDto = airportsDto.OrderByDescending(a => a.Name);
                    break;
                case "location":
                    airportsDto = airportsDto.OrderBy(a => a.Location);
                    break;
                case "location_desc":
                    airportsDto = airportsDto.OrderByDescending(a => a.Location);
                    break;
                default:
                    airportsDto = airportsDto.OrderBy(a => a.Name);
                    break;
            }

            var viewModel = new AirportViewModel
            {
                Airports = airportsDto.ToList(),
                SearchTerm = searchTerm,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AirportsForCreationDto airportForCreation)
        {
            if (ModelState.IsValid)
            {
                await _airportService.Create(airportForCreation);
                return RedirectToAction("Index");
            }
            return View(airportForCreation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var airport = await _airportService.GetById(id);
            if (airport == null) return NotFound();

            var airportForUpdate = new AirportsForUpdateDto
            {
                AirportID = airport.AirportID,
                Name = airport.Name,
                Location = airport.Location,
            };
            return View(airportForUpdate);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(AirportsForUpdateDto airportForUpdate)
        {
            if (ModelState.IsValid)
            {
                var result = await _airportService.Update(airportForUpdate);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return View(airportForUpdate);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _airportService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
