using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.BLL.Services;
using FlightManagement.DAL;
using FlightManagement.DTO.Flights;
using FlightManagement.DTO.Rotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.ASPnet.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly IRouteService _routeService;
        private readonly IStopService _stopService;
        private readonly ILogger<FlightController> _logger;
        private readonly AppDbContext _context;
        

        public FlightController(AppDbContext context, IFlightService flightService, IRouteService routeService, IStopService stopService, ILogger<FlightController> logger)
        {
            _routeService = routeService;
            _stopService = stopService;
            _context = context;
            _flightService = flightService;
            
        }

        // [ResponseCache(Duration = 272)]
        public async Task<IActionResult> Index(string airportName = null, string routeID = null, TimeSpan? departureTime = null)
        {
            var flights = await _flightService.GetAll();
            // Логика фильтрации рейсов по аэропорту, маршруту и времени вылета
            // Пример: фильтрация по airportName и т. д.
            return View(flights);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Подготовка данных для создания нового рейса
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(FlightsForCreationDto flightDto)
        {
            if (ModelState.IsValid)
            {
                await _flightService.Create(flightDto);
                return RedirectToAction(nameof(Index));
            }

            // Логируем ошибки валидации
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError(error.ErrorMessage);
            }

            return View(flightDto);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var flight = await _flightService.GetById(id);
            if (flight == null) return NotFound();

            // Подготовка данных для редактирования рейса
            return View(flight);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(FlightsForUpdateDto flightDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _flightService.Update(flightDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Не удалось обновить рейс.");
            }

            return View(flightDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _flightService.GetById(id);
            if (flight == null) return NotFound();

            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _flightService.Delete(id);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRoutes(int flightId)
        {
            var routes = await _routeService.GetAll(); // Получаем все маршруты
            ViewBag.FlightId = flightId;
            return View(routes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoute(RoutesForCreationDto routeDto)
        {
            if (ModelState.IsValid)
            {
                await _routeService.Create(routeDto);
                return RedirectToAction(nameof(ManageRoutes), new { flightId = routeDto.FlightID });
            }

            return View(routeDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRoute(int id)
        {
            var route = await _routeService.GetById(id);
            if (route == null) return NotFound();

            return View(route);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRoute(RoutesForUpdateDto routeDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _routeService.Update(routeDto);
                if (result)
                {
                    return RedirectToAction(nameof(ManageRoutes), new { flightId = routeDto.FlightID });
                }
                ModelState.AddModelError("", "Не удалось обновить маршрут.");
            }

            return View(routeDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _routeService.GetById(id);
            if (route == null) return NotFound();

            return View(route);
        }

        [HttpPost, ActionName("DeleteRoute")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRouteConfirmed(int id)
        {
            var result = await _routeService.Delete(id);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
