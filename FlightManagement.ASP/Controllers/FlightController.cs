
using Microsoft.AspNetCore.Mvc;
using FlightManagement.DAL;
using FlightManagement.models;
using System.Linq;

public class FlightController : Controller
{
    private readonly AppDbContext _context;

    public FlightController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var flights = _context.Flights.ToList();
        return View(flights);
    }

    public IActionResult Details(int id)
    {
        var flight = _context.Flights.Find(id);
        if (flight == null)
        {
            return NotFound();
        }
        return View(flight);
    }
}
