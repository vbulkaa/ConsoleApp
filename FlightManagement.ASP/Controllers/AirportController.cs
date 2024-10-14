using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightManagement.DAL;

namespace FlightManagement.ASP.Controllers
{

    public class AirportController : Controller
    {
        private readonly AppDbContext _context;

        public AirportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var airports = _context.Airports.ToList();
            return View(airports);
        }

        public IActionResult Details(int id)
        {
            var airport = _context.Airports.Find(id);
            if (airport == null)
            {
                return NotFound();
            }
            return View(airport);
        }
    }
}
