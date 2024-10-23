using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Rotes;
using FlightManagement.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
            flights = sortOrder switch
            {
                "date_desc" => flights.OrderByDescending(f => f.Routes.FirstOrDefault().Date).ToList(),
                _ => flights.OrderBy(f => f.Routes.FirstOrDefault().Date).ToList(),
            };//не работает с пустыми значениями

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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int routeID)
        {
            var route = await _routeService.GetById(routeID);
            if (route == null)
            {
                return NotFound();
            }

            var flights = await _flightService.GetAll();
            ViewBag.Flights = flights;

         
            // Получение списока аэропортов
            var airports = await _airportService.GetAll();
            ViewBag.Airports = airports;

            var statuses = await _statusService.GetAll();
            ViewBag.Statuses = statuses;

            // Переписываем DTO, чтобы включить остановки
            var routeDto = new RoutesForUpdateDto
            {
                RouteID = route.RouteID,
                FlightID = route.FlightID,
                DepartureTime = route.DepartureTime,
                Date = route.Date,
                Stops = (route.Stops ?? new List<Stops>()).Select(s => new StopsForUpdateDto
                {
                    StopID = s.StopID,
                    RouteID = s.RouteID,
                    AirportID = s.AirportID,
                    ArrivalTime = s.ArrivalTime,
                    DepartureTime = s.DepartureTime,
                    StatusID = s.StatusID
                }).ToList()
            };

            return View(routeDto);
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(RoutesForUpdateDto routesForUpdateDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(routesForUpdateDto);
        //    }

        //    // Получение существующего маршрута для обновления остановок
        //    var existingRoute = await _routeService.GetById(routesForUpdateDto.RouteID);
        //    if (existingRoute == null)
        //    {
        //        return NotFound();
        //    }

        //    // Обновление существующих остановок
        //    foreach (var stop in routesForUpdateDto.Stops)
        //    {
        //        var existingStop = existingRoute.Stops.FirstOrDefault(s => s.StopID == stop.StopID);
        //        if (existingStop != null)
        //        {
        //            existingStop.AirportID = stop.AirportID;
        //            existingStop.ArrivalTime = stop.ArrivalTime;
        //            existingStop.DepartureTime = stop.DepartureTime;
        //            existingStop.StatusID = stop.StatusID;
        //        }
        //        else
        //        {
        //            // Если остановка новая, добавляем ее
        //            existingRoute.Stops.Add(new Stops
        //            {
        //                AirportID = stop.AirportID,
        //                ArrivalTime = stop.ArrivalTime,
        //                DepartureTime = stop.DepartureTime,
        //                StatusID = stop.StatusID
        //            });
        //        }
        //    }

        //    // Сохранение изменений
        //    await _repositoryManager.SaveAsync();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(RoutesForUpdateDto routesForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(routesForUpdateDto);
            }

            // Получение существующего маршрута для обновления остановок
            var existingRoute = await _routeService.GetById(routesForUpdateDto.RouteID);
            if (existingRoute == null)
            {
                return NotFound();
            }

            // Обновление существующих остановок
            foreach (var stop in routesForUpdateDto.Stops)
            {
                var existingStop = existingRoute.Stops.FirstOrDefault(s => s.StopID == stop.StopID);
                if (existingStop != null)
                {
                    existingStop.AirportID = stop.AirportID;
                    existingStop.ArrivalTime = stop.ArrivalTime;
                    existingStop.DepartureTime = stop.DepartureTime;
                    existingStop.StatusID = stop.StatusID;
                }
                else
                {
                    // Если остановка новая, добавляем ее
                    existingRoute.Stops.Add(new Stops
                    {
                        AirportID = stop.AirportID,
                        ArrivalTime = stop.ArrivalTime,
                        DepartureTime = stop.DepartureTime,
                        StatusID = stop.StatusID
                    });
                }
            }

            // Удаление удаленных остановок
            var stopsToRemove = existingRoute.Stops.Where(s => !routesForUpdateDto.Stops.Any(dto => dto.StopID == s.StopID)).ToList();
            foreach (var stopToRemove in stopsToRemove)
            {
                existingRoute.Stops.Remove(stopToRemove);
            }

            // Сохранение изменений
            await _repositoryManager.SaveAsync();
            return RedirectToAction("Index");
        }

        
        // Удаление маршрута
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoute(int routeID)
        {
            var deleted = await _routeService.Delete(routeID);
            if (!deleted)
            {
                return NotFound();
            }

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


        

       
        
    }
}

