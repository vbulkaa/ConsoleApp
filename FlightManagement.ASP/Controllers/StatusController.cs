﻿using Microsoft.AspNetCore.Mvc;
using FlightManagement.DAL;

namespace FlightManagement.ASP.Controllers
{
    public class StatusController : Controller
    {
        private readonly AppDbContext _context;

        public StatusController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var statuses = _context.Statuses.ToList();
            return View(statuses);
        }

        public IActionResult Details(int id)
        {
            var status = _context.Statuses.Find(id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }
    }
}
