using FlightManagement.DAL.Repositories.Base;
using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Repositories
{
    public class AirportsRepository : RepositoryBase<Airports>, IAirportsRepository
    {
        private readonly IMemoryCache _memoryCache; 
        private readonly AppDbContext _dbContext;       

public AirportsRepository(AppDbContext dbcontext, IMemoryCache memoryCache) 
            : base(dbcontext, memoryCache) 
        {
            _dbContext = dbContext;
        }

        /*public IQueryable<Airports> GetAllEntities(bool trackChanges)
        {
            return !trackChanges ? dbContext.Airports.AsNoTracking() : dbContext.Airports;
        }*/

        public IQueryable<Airports> GetAllEntities(bool trackChanges)
        {
            return !trackChanges ?
                _dbContext.Airports.AsNoTracking() :
                _dbContext.Airports;
        }

        public async Task Create(Airports entity)
        {
            await _dbContext.Airports.AddAsync(entity); // Добавляем аэропорт
           // await _dbContext.SaveChangesAsync();  
        }
        
        public async Task Delete(Airports entity)
        {

            DeleteEntity(entity);
           // await _dbContext.SaveChangesAsync();
        }
           

        public async Task<IEnumerable<Airports>> GetAll(bool trackChanges) =>
     await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Airports> GetById(int id, bool trackChanges) =>
            await GetByCondition(a => a.AirportID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Airports entity)
        {
            UpdateEntity(entity);
           
        }

        public async Task<IEnumerable<Airports>> Get(int rowsCount, string cacheKey)
        {
            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Airports> entities))
            {
                entities = await dbContext.Airports.Take(rowsCount).Include(e => e.AirportID).ToListAsync();
                if (entities != null)
                {
                    memoryCache.Set(cacheKey, entities,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CachingTime)));
                }
            }
            return entities;
        }
    }
}
