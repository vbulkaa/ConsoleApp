using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IAirportsRepository AirportsRepository
        {
            get
            {
                if (_airportsRepository == null)
                {
                    _airportsRepository = new AirportsRepository(_dbContext, _memoryCache);
                }
                return _airportsRepository;
            }
        }

        public IFlightsRepository FlightsRepository
        {
            get
            {
                if (_flightsRepository == null)
                {
                    _flightsRepository = new FlightsRepository(_dbContext, _memoryCache);
                }
                return _flightsRepository;
            }
        }

        public IRoutesRepository RoutesRepository
        {
            get
            {
                if (_routesRepository == null)
                {
                    _routesRepository = new RoutesRepository(_dbContext);
                }
                return _routesRepository;
            }
        }

        public IStatusesRepository StatusesRepository
        {
            get
            {
                if (_statusesRepository == null)
                {
                    _statusesRepository = new StatusesRepository(_dbContext);
                }
                return _statusesRepository;
            }
        }

        public IStopsRepository StopsRepository
        {
            get
            {
                if (_stopsRepository == null)
                {
                    _stopsRepository = new StopsRepository(_dbContext);
                }
                return _stopsRepository;
            }
        }
    }
}
