using FlightManagement.ASPnet.Models;
using FlightManagement.BLL.Services;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Flights;
using Microsoft.EntityFrameworkCore;

using FlightManagement.DAL.models;
using FlightManagement.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightManagement.DAL.models.enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlightManagement.DAL;

namespace FlightManagement.ASPnet.Controllers
{
    public class FlightController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IRepositoryManager repository, ILogger<FlightController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string airportName = null, string routeID = null, TimeSpan? departureTime = null, int page = 1, int pageSize = 10)
        {
            var departureSchedules = await GetFlightSchedulesByStatus("вылет");
            var arrivalSchedules = await GetFlightSchedulesByStatus("прилет");

            var flightSchedules = departureSchedules.Concat(arrivalSchedules)
                                                     .Distinct(new FlightScheduleComparer())
                                                     .ToList();

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

            // Calculate total pages
            int totalCount = flightSchedules.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            flightSchedules = flightSchedules.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Passing pagination data to the view
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;

            return View(flightSchedules.OrderBy(fs => fs.DepartureTime).ToList());
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

        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> IndexAdmin()
        //{
        //    var flights = await _repository.FlightsRepository.GetAllEntities(false).ToListAsync();
        //    return View(flights);
        //}
        public async Task<IActionResult> IndexAdmin(int page = 1, int pageSize = 10)
        {
            // Получаем все рейсы как IQueryable
            var flightsQuery = _repository.FlightsRepository.GetAllEntities(false);

            // Получаем общее количество рейсов
            var totalCount = await flightsQuery.CountAsync();

            // Рассчитываем общее количество страниц
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Получаем рейсы для текущей страницы
            var pagedFlights = await flightsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Передаем данные в представление
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;

            return View(pagedFlights);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Заполните ViewBag данными для выпадающего списка типов самолета
            ViewBag.AircraftTypes = Enum.GetValues(typeof(AircraftType)).Cast<AircraftType>().Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetDisplayName() // Извлекаем отображаемое имя
            }).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(FlightsForCreationDto flightDto)
        {
            if (ModelState.IsValid)
            {
                // Маппинг DTO на модель Flights
                var flight = new Flights
                {
                    FlightNumber = flightDto.FlightNumber,
                    AircraftType = flightDto.AircraftType,
                    TicketPrice = flightDto.TicketPrice
                };

                await _repository.FlightsRepository.Create(flight);
                await _repository.SaveAsync();
                return RedirectToAction(nameof(IndexAdmin));
            }

            // Логируем ошибки валидации
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError(error.ErrorMessage); // Логируем ошибку
            }

            // Заполнение ViewBag снова
            ViewBag.AircraftTypes = Enum.GetValues(typeof(AircraftType)).Cast<AircraftType>().Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetDisplayName()
            }).ToList();

            return View(flightDto); // Возвращаем DTO обратно в представление
        }

       // Добавьте это пространство имен

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var flight = await _repository.FlightsRepository.GetById(id, false);
        if (flight == null) return NotFound();

        // Создание DTO для передачи данных в представление
        var flightDto = new FlightsForUpdateDto
        {
            FlightID = flight.FlightID,
            FlightNumber = flight.FlightNumber,
            AircraftType = flight.AircraftType,
            TicketPrice = flight.TicketPrice
        };

        // Заполните ViewBag данными для выпадающего списка типов самолета
        ViewBag.AircraftTypes = Enum.GetValues(typeof(AircraftType)).Cast<AircraftType>().Select(e => new SelectListItem
        {
            Value = e.ToString(),
            Text = e.GetDisplayName() // Извлекаем отображаемое имя
        }).ToList();

        return View(flightDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(FlightsForUpdateDto flightDto)
    {
        if (ModelState.IsValid)
        {
            // Маппинг DTO на сущность
            var flight = new Flights
            {
                FlightID = flightDto.FlightID,
                FlightNumber = flightDto.FlightNumber,
                AircraftType = flightDto.AircraftType,
                TicketPrice = flightDto.TicketPrice
            };

            _repository.FlightsRepository.Update(flight);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(IndexAdmin));
        }

        // В случае ошибки, верните View с заполненными данными
        ViewBag.AircraftTypes = Enum.GetValues(typeof(AircraftType)).Cast<AircraftType>().Select(e => new SelectListItem
        {
            Value = e.ToString(),
            Text = e.GetDisplayName() // Извлекаем отображаемое имя
        }).ToList();

        return View(flightDto);
    }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _repository.FlightsRepository.GetById(id, trackChanges: false);
            if (flight == null) return NotFound();

            return View(flight);
        }


        
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int flightID)
        {
            // Получаем рейс, чтобы убедиться, что он существует
            var flight = await _repository.FlightsRepository.GetById(flightID, trackChanges: false);
            if (flight == null) return NotFound();

            // Получаем все маршруты, связанные с рейсом
            var routes = await _repository.RoutesRepository.GetByCondition(
                r => r.FlightID == flightID,
                trackChanges: false).ToListAsync();

            // Удаляем все остановки, связанные с маршрутами
            if (routes.Any())
            {
                // Получаем все остановки для всех маршрутов
                var stops = await _repository.StopsRepository.GetByCondition(
                    s => routes.Select(r => r.RouteID).Contains(s.RouteID),
                    trackChanges: false).ToListAsync();

                if (stops.Any())
                {
                    await _repository.StopsRepository.DeleteRange(stops); // Асинхронное удаление остановок
                }

                await _repository.RoutesRepository.DeleteRange(routes); // Асинхронное удаление маршрутов
            }

            // Удаляем сам рейс
            await _repository.FlightsRepository.Delete(flight);
            await _repository.SaveAsync(); // Сохраняем изменения в базе данных

            return RedirectToAction(nameof(IndexAdmin));
        }

    }
}
