using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.DAL
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        private IAirportsRepository _airportsRepository;
        private IFlightsRepository _flightsRepository;
        private IRoutesRepository _routesRepository;
        private IStatusesRepository _statusesRepository;
        private IStopsRepository _stopsRepository;

        public RepositoryManager(AppDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public IAirportsRepository AirportsRepository =>
             _airportsRepository ??= new AirportsRepository(_dbContext, _memoryCache);

        public IFlightsRepository FlightsRepository =>
            _flightsRepository ??= new FlightsRepository(_dbContext, _memoryCache);

        public IRoutesRepository RoutesRepository =>
            _routesRepository ??= new RoutesRepository(_dbContext, _memoryCache);

        public IStatusesRepository StatusesRepository =>
            _statusesRepository ??= new StatusesRepository(_dbContext, _memoryCache);

        public IStopsRepository StopsRepository =>
            _stopsRepository ??= new StopsRepository(_dbContext, _memoryCache);

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
