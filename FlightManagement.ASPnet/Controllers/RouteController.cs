using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Rotes;
using FlightManagement.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DTO.Flights;
using FlightManagement.DAL;
using Microsoft.Data.SqlClient;
using FlightManagement.DTO.Stops;
using FlightManagement.BLL.Services;
using System.Linq; // Для методов LINQ
using System.Collections.Generic; // Для работы с коллекциями

namespace FlightManagement.Controllers
{
    public class RouteController : Controller
    {
        private readonly ILogger<RouteController> _logger;
        private readonly IRouteService _routeService;
        private readonly IFlightService _flightService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IAirportService _airportService;
        private readonly IStatusService _statusService;
        public RouteController(ILogger<RouteController> logger,IRouteService routeService, IStatusService statusService, IFlightService flightService, IRepositoryManager repositoryManager, IAirportService airportService)
        {
            _routeService = routeService;
            _flightService = flightService;
            _airportService = airportService;
            _statusService = statusService;
            _repositoryManager = repositoryManager;
            _logger = logger;

        }

        // Получение списка всех маршрутов
        public async Task<IActionResult> Index(string flightNumber, DateTime? departureDate, string sortOrder)
        {
            var flights = await _repositoryManager.FlightsRepository.GetAllEntities(false)
               .Select(f => new
               {

                   f.FlightID,
                   f.FlightNumber,
                   Routes = f.Routes.Select(r => new
                   {
                       r.RouteID,
                       r.DepartureTime,
                       r.Date
                   }).ToList()
               }).ToListAsync();

            // Фильтрация
            if (!string.IsNullOrEmpty(flightNumber))
            {
                flights = flights.Where(f => f.FlightNumber.Contains(flightNumber, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (departureDate.HasValue)
            {
                flights = flights.Where(f => f.Routes.Any(r => r.Date.Date == departureDate.Value.Date)).ToList();
            }

            // Сортировка
            /*flights = sortOrder switch
            {

                "date_desc" => flights.OrderByDescending(f => f.Routes.FirstOrDefault().Date).ToList(),
                _ => flights.OrderBy(f => f.Routes.FirstOrDefault().Date).ToList(),
            };//не работает с пустыми значениями*/

            flights = sortOrder switch
            {
                "date_desc" => flights
                    .OrderByDescending(f => f.Routes.FirstOrDefault()?.Date ?? DateTime.MaxValue)
                    .ToList(),
                _ => flights
                    .OrderBy(f => f.Routes.FirstOrDefault()?.Date ?? DateTime.MaxValue)
                    .ToList(),
            };

            // Передача значений фильтров в ViewData
            ViewData["CurrentFlightNumber"] = flightNumber;
            ViewData["CurrentDepartureDate"] = departureDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentSort"] = sortOrder;

            return View(flights);

        }

        // Создание маршрута
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var flights = await _repositoryManager.FlightsRepository.GetAll(false);
            var airports = await _repositoryManager.AirportsRepository.GetAll(false);
            var statuses = await _repositoryManager.StatusesRepository.GetAll(false);

            ViewBag.Flights = flights;
            ViewBag.Airports = airports;
            ViewBag.Statuses = statuses;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RoutesForCreationDto routeDto)
        {
            if (!ModelState.IsValid)
            {
                // Повторно заполняем данные, если состояние модели недействительно
                var flights = await _repositoryManager.FlightsRepository.GetAll(false);
                var airports = await _repositoryManager.AirportsRepository.GetAll(false);
                var statuses = await _repositoryManager.StatusesRepository.GetAll(false);

                ViewBag.Flights = flights;
                ViewBag.Airports = airports;
                ViewBag.Statuses = statuses;

                return View(routeDto);
            }

            // Преобразуем DTO в сущность Route и сохраняем в базе данных
            var route = new Routes
            {
                FlightID = routeDto.FlightID,
                DepartureTime = routeDto.DepartureTime,
                Date = routeDto.Date,
                Stops = routeDto.Stops.Select(s => new Stops
                {
                    AirportID = s.AirportID,
                    ArrivalTime = s.ArrivalTime,
                    DepartureTime = s.DepartureTime,
                    StatusID = s.StatusID
                }).ToList()
            };

            // Убедитесь, что метод репозитория соответствует вашей реализации
            await _repositoryManager.RoutesRepository.Create(route);
            await _repositoryManager.SaveAsync();

            return RedirectToAction("Index");
        }
       

        // Отображение деталей маршрута
        public async Task<IActionResult> RouteDetails(int routeID)
        {
            var routeDetails = await _repositoryManager.RoutesRepository.GetAllEntities(false)
                .Where(r => r.RouteID == routeID)
                .Select(r => new
                {
                    r.RouteID,
                    r.DepartureTime,
                    r.Date,
                    Stops = r.Stops.Select(s => new
                    {
                        s.StopID,
                        s.ArrivalTime,
                        s.DepartureTime,
                        AirportName = s.Airport.Name,
                        AirportLocation = s.Airport.Location,
                        StatusName = s.Status.StatusName
                    }).OrderBy(s => s.DepartureTime).ToList()
                })
                .FirstOrDefaultAsync();

            if (routeDetails == null)
            {
                return NotFound();
            }

            return View(routeDetails);
        }

        //// Отображение формы редактирования маршрута
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int routeID)
        //{
        //    var flights = await _repositoryManager.FlightsRepository.GetAll(false);
        //    var airports = await _repositoryManager.AirportsRepository.GetAll(false);
        //    var statuses = await _repositoryManager.StatusesRepository.GetAll(false);
        //    var stops = await _repositoryManager.StopsRepository.GetAll(false);

        //    var route = await _repositoryManager.RoutesRepository
        //            .GetByCondition(r => r.RouteID == routeID, trackChanges: false) // Получаем IQueryable
        //            .Include(r => r.Stops) // Теперь можно использовать Include
        //            .FirstOrDefaultAsync();
        //    if (route == null)
        //    {
        //        return NotFound();
        //    }

        //    var routesForUpdateDto = new RoutesForUpdateDto
        //    {
        //        RouteID = route.RouteID,
        //        FlightID = route.FlightID,
        //        DepartureTime = route.DepartureTime,
        //        Date = route.Date,
        //        Stops = route.Stops.Select(s => new StopsForUpdateDto
        //        {
        //            StopID = s.StopID,
        //            AirportID = s.AirportID,
        //            ArrivalTime = s.ArrivalTime,
        //            DepartureTime = s.DepartureTime,
        //            StatusID = s.StatusID
        //        }).ToList()
        //    };



        //    ViewBag.Flights = flights;
        //    ViewBag.Airports = airports;
        //    ViewBag.Statuses = statuses;

        //    return View(routesForUpdateDto);
        //}

        // Обработка редактирования маршрута
       
        public async Task<IActionResult> Edit(int routeID)
        {
            var flights = await _repositoryManager.FlightsRepository.GetAll(false);
            var airports = await _repositoryManager.AirportsRepository.GetAll(false);
            var statuses = await _repositoryManager.StatusesRepository.GetAll(false);
            var stops = await _repositoryManager.StopsRepository.GetAll(false);

            var route = await _repositoryManager.RoutesRepository
                    .GetByCondition(r => r.RouteID == routeID, trackChanges: false)
                    .Include(r => r.Stops)
                    .FirstOrDefaultAsync();

            if (route == null)
            {
                return NotFound();
            }

            var routesForUpdateDto = new RoutesForUpdateDto
            {
                RouteID = route.RouteID,
                FlightID = route.FlightID,
                DepartureTime = route.DepartureTime,
                Date = route.Date,
                Stops = route.Stops.Select(s => new StopsForUpdateDto
                {
                    StopID = s.StopID,
                    AirportID = s.AirportID,
                    ArrivalTime = s.ArrivalTime,
                    DepartureTime = s.DepartureTime,
                    StatusID = s.StatusID
                }).ToList()
            };

            ViewBag.Flights = flights;
            ViewBag.Airports = airports;
            ViewBag.Statuses = statuses;

            return View(routesForUpdateDto);
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(RoutesForUpdateDto routesForUpdateDto, int routeID)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Повторно заполняем данные, если состояние модели недействительно
        //        var flights = await _repositoryManager.FlightsRepository.GetAll(false);
        //        var airports = await _repositoryManager.AirportsRepository.GetAll(false);
        //        var statuses = await _repositoryManager.StatusesRepository.GetAll(false);

        //        ViewBag.Flights = flights;
        //        ViewBag.Airports = airports;
        //        ViewBag.Statuses = statuses;

        //        return View(routesForUpdateDto);
        //    }

        //    var route = await _repositoryManager.RoutesRepository.GetById(routesForUpdateDto.RouteID, trackChanges: true);
        //    if (route == null)
        //    {
        //        return NotFound();
        //    }

        //    route.FlightID = routesForUpdateDto.FlightID;
        //    route.DepartureTime = routesForUpdateDto.DepartureTime;
        //    route.Date = routesForUpdateDto.Date;

        //    var route_stop = await _repositoryManager.RoutesRepository
        //             .GetByCondition(r => r.RouteID == routeID, trackChanges: false) // Получаем IQueryable
        //             .Include(r => r.Stops) // Теперь можно использовать Include
        //             .FirstOrDefaultAsync();

        //    var existingStops = route_stop.Stops.ToList();
        //    foreach (var stopDto in existingStops)
        //    {
        //        if (!routesForUpdateDto.Stops.Any(s => s.StopID == stopDto.StopID))
        //        {
        //            _repositoryManager.StopsRepository.Delete(stopDto);
        //        }
        //    }
        //    foreach (var stopDto in routesForUpdateDto.Stops)
        //    {
        //        var existingstop = existingStops.FirstOrDefault(s => s.StopID == stopDto.StopID);
        //        if (existingstop != null)
        //        {
        //            route.Stops = routesForUpdateDto.Stops.Select(s => new Stops
        //            {
        //                StopID = s.StopID,
        //                AirportID = s.AirportID,
        //                ArrivalTime = s.ArrivalTime,
        //                DepartureTime = s.DepartureTime,
        //                StatusID = s.StatusID
        //            }).ToList();
        //        }

        //    }
        //    await _repositoryManager.SaveAsync();

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(RoutesForUpdateDto routesForUpdateDto, int routeID)
        {
            if (!ModelState.IsValid)
            {
                // Повторно заполняем данные, если состояние модели недействительно
                var flights = await _repositoryManager.FlightsRepository.GetAll(false);
                var airports = await _repositoryManager.AirportsRepository.GetAll(false);
                var statuses = await _repositoryManager.StatusesRepository.GetAll(false);

                ViewBag.Flights = flights;
                ViewBag.Airports = airports;
                ViewBag.Statuses = statuses;

                return View(routesForUpdateDto);
            }

            var route = await _repositoryManager.RoutesRepository.GetById(routeID, trackChanges: true);
            if (route == null)
            {
                return NotFound();
            }

            route.FlightID = routesForUpdateDto.FlightID;
            route.DepartureTime = routesForUpdateDto.DepartureTime;
            route.Date = routesForUpdateDto.Date;

            // Получаем существующие остановки
            var routeWithStops = await _repositoryManager.RoutesRepository
                .GetByCondition(r => r.RouteID == routeID, trackChanges: true)
                .Include(r => r.Stops)
                .FirstOrDefaultAsync();

            // Удаляем остановки, которые не присутствуют в обновленных данных
            foreach (var stop in routeWithStops.Stops.ToList())
            {
                if (!routesForUpdateDto.Stops.Any(s => s.StopID == stop.StopID))
                {
                    _repositoryManager.StopsRepository.Delete(stop);
                }
            }

            // Обновляем или добавляем остановки
            foreach (var stopDto in routesForUpdateDto.Stops)
            {
                var existingStop = routeWithStops.Stops.FirstOrDefault(s => s.StopID == stopDto.StopID);
                if (existingStop != null)
                {
                    // Обновляем существующую остановку
                    existingStop.AirportID = stopDto.AirportID;
                    existingStop.ArrivalTime = stopDto.ArrivalTime;
                    existingStop.DepartureTime = stopDto.DepartureTime;
                    existingStop.StatusID = stopDto.StatusID;
                }
                else
                {
                    // Добавляем новую остановку
                    var newStop = new Stops
                    {
                        AirportID = stopDto.AirportID,
                        ArrivalTime = stopDto.ArrivalTime,
                        DepartureTime = stopDto.DepartureTime,
                        StatusID = stopDto.StatusID
                    };
                    routeWithStops.Stops.Add(newStop);
                }
            }

            await _repositoryManager.SaveAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int routeID)
        {
            // Получаем маршрут по его ID
            var route = await _repositoryManager.RoutesRepository.GetById(routeID, trackChanges: false);
            if (route != null)
            {
                // Получаем все остановки, связанные с маршрутом
                var stops = await _repositoryManager.StopsRepository.GetAllEntities(false)
                    .Where(s => s.RouteID == route.RouteID)
                    .ToListAsync();

                if (stops.Any())
                {
                    await _repositoryManager.StopsRepository.DeleteRangeAsync(stops); // Удаляем связанные остановки
                }

                // Удаляем маршрут
                await _repositoryManager.RoutesRepository.DeleteRangeAsync(new List<Routes> { route });
            }

            await _repositoryManager.SaveAsync(); // Сохраняем изменения в базе данных

            return RedirectToAction(nameof(Index));
        }


    }
}

