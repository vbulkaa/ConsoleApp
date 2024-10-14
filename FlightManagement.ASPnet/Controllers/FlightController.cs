using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.ASPnet.Controllers
{
    public class FlightController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFlightService _flightService;

        public FlightController(AppDbContext context, IFlightService flightService)
        {
            _context = context;
            _flightService = flightService;
        }

        [ResponseCache(Duration = 272)]
        public IActionResult Index()
        {
            var flights = _context.Flights.ToList();
            return View(flights);
        }

        [ResponseCache(Duration = 272)]
        public async Task<IActionResult> GetAll()
        {
            return View(await _flightService.Get(50, "Flight50"));
        }
    }
}
