﻿using FlightManagement.models;
using FlightManagement.data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Collections.Generic;
using System.Linq;

public class Query
{
    private readonly AppDbContext _context;

    public Query(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        _context = new AppDbContext(builder.UseSqlServer(connectionString).Options);

    }

    public List<Airports> GetAllAirports()
    {
        return _context.Airports.ToList();
    }

    public List<Airports> GetFilteredAirports(string name)
    {
        return _context.Airports.Where(a => a.Name.Contains(name)).ToList();
    }

    public int GetRouteCountByFlight(int flightId)
    {
        return _context.Routes.Count(r => r.FlightID == flightId);
    }

    public List<(string FlightNumber, DateTime RouteDate)> GetFlightRoutes()
    {
        return _context.Flights
            .Include(f => f.Routes)
            .SelectMany(f => f.Routes.Select(r => new
            {
                FlightNumber = f.FlightNumber,
                RouteDate = r.Date
            }))
            .AsEnumerable() // Преобразуем в IEnumerable, чтобы избежать проблемы с LINQ
            .Select(x => (x.FlightNumber, x.RouteDate)) // Преобразуем в кортежи
            .ToList();
    }

    public List<Stops> GetFilteredStopsByFlight(int flightId)
    {
        return _context.Stops
            .Include(s => s.Route)
            .Where(s => s.Route.FlightID == flightId)
            .ToList();
    }

    public void AddAirport(Airports airport)
    {
        _context.Airports.Add(airport);
        _context.SaveChanges();
    }

    public void AddStop(Stops stop)
    {
        _context.Stops.Add(stop);
        _context.SaveChanges();
    }

    public void DeleteAirport(int airportId)
    {
       var stopsToDelete = _context.Stops.Where(s => s.AirportID == airportId).ToList();

        _context.Stops.RemoveRange(stopsToDelete);

        var airport = _context.Airports.Find(airportId);
        if (airport != null)
        {
            _context.Airports.Remove(airport);
            _context.SaveChanges();
        }

    }

    public void DeleteStop(int stopId)
    {
        var stop = _context.Stops.Find(stopId);
        if (stop != null)
        {
            _context.Stops.Remove(stop);
            _context.SaveChanges();
        }
    }

    public void UpdateFlightTicketPrice(int flightId, decimal newPrice)
    {
        var flight = _context.Flights.Find(flightId);
        if (flight != null)
        {
            flight.TicketPrice = newPrice;
            _context.SaveChanges();
        }
    }
}
