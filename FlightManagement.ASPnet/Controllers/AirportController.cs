﻿using FlightManagement.DAL;
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
        public async Task<IActionResult> Index(string searchTerm, string sortOrder, int page = 1, int pageSize = 10)
        {
            var airports = await _airportService.GetAll();
            var airportsDto = _mapper.Map<IEnumerable<AirportsDto>>(airports);

            // Поиск
            if (!string.IsNullOrEmpty(searchTerm))
            {
                airportsDto = airportsDto.Where(a => a.Name.Contains(searchTerm) || a.Location.Contains(searchTerm));
            }

            // Сортировка
            airportsDto = sortOrder switch
            {
                "airportID_desc" => airportsDto.OrderByDescending(a => a.AirportID),
                "airportID" => airportsDto.OrderBy(a => a.AirportID),
                "name_desc" => airportsDto.OrderByDescending(a => a.Name),
                "name" => airportsDto.OrderBy(a => a.Name),
                "location_desc" => airportsDto.OrderByDescending(a => a.Location),
                "location" => airportsDto.OrderBy(a => a.Location),
                _ => airportsDto.OrderBy(a => a.Name),
            };

            // Пагинация
            int totalCount = airportsDto.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginatedAirports = airportsDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new AirportViewModel
            {
                Airports = paginatedAirports,
                SearchTerm = searchTerm,
                SortOrder = sortOrder
            };

            // Передача значений пагинации в ViewData
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;

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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var airport = await _airportService.GetById(id);
            if (airport == null)
            {
                return NotFound(); // Return 404 if the airport is not found
            }

            var airportDto = new AirportsDto
            {
                AirportID = airport.AirportID,
                Name = airport.Name,
                Location = airport.Location
            };

            return View(airportDto); // Return the view with the airport details
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
