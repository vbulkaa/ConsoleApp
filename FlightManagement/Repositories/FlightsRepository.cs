using FlightManagement.DAL.Repositories.Base;
using FlightManagement.models;
using FlightManagement.DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Repositories
{
    public class FlightsRepository : RepositoryBase<Flight>, IFlightsRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public FlightsRepository(AppDbContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        {
            _dbContext = context;
        }



        public async Task Create(Flight entity)
        {
            await _dbContext.Flights.AddAsync(entity);
        }

        public async Task Delete(Flight entity)
        {
            _dbContext.Flights.RemoveRange(entity);

        }

        public async Task<IEnumerable<Flight>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).Include(f => f.Routes).ToListAsync();

        public async Task<Flight> GetById(int id, bool trackChanges) =>
            await GetByCondition(f => f.FlightID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Flight entity)
        {
            _dbContext.Set<Flight>().Update(entity);
        }

        public async Task<IEnumerable<Flight>> Get(int rowsCount, string cacheKey)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Flight> entities))
            {
                entities = await dbContext.Flights.Take(rowsCount).ToListAsync();
                if (entities != null)
                {
                    _memoryCache.Set(cacheKey, entities,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CachingTime)));
                }
            }
            return entities;
        }

        
    }
}
