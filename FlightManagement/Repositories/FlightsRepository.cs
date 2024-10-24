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
    public class FlightsRepository : RepositoryBase<Flights>, IFlightsRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public FlightsRepository(AppDbContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        {
            _dbContext = context;
        }



        //public FlightsRepository(AppDbContext dbContext, IMemoryCache memoryCache)
        //    : base(dbContext)
        //{
        //    _memoryCache = memoryCache;
        //}

        public async Task Create(Flights entity)
        {
            await _dbContext.Flights.AddAsync(entity);
        }

        public async Task Delete(Flights entity)
        {
            _dbContext.Flights.RemoveRange(entity);

        }

        public async Task<IEnumerable<Flights>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).Include(f => f.Routes).ToListAsync();

        public async Task<Flights> GetById(int id, bool trackChanges) =>
            await GetByCondition(f => f.FlightID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Flights entity)
        {
            _dbContext.Set<Flights>().Update(entity);
        }

        public async Task<IEnumerable<Flights>> Get(int rowsCount, string cacheKey)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Flights> entities))
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
