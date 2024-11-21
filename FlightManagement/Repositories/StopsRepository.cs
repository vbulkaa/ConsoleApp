using FlightManagement.DAL.Repositories.Base;
using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace FlightManagement.DAL.Repositories
{
    public class StopsRepository : RepositoryBase<Stop>, IStopsRepository
    {
        public StopsRepository(AppDbContext context, IMemoryCache memoryCache) : base(context, memoryCache) { }
        //public StopsRepository(AppDbContext dbContext)
        //    : base(dbContext)
        //{
        //}

        public async Task Create(Stop entity) =>
            await CreateEntity(entity);

        public async Task Delete(Stop entity)
        {

            DeleteEntity(entity);
            // await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Stop>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Stop> GetById(int id, bool trackChanges) =>
            await GetByCondition(s => s.StopID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Stop entity) =>
            await Update(entity);

        public async Task<IEnumerable<Stop>> Get(int rowsCount, string cacheKey)
        {
            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Stop> entities))
            {
                entities = await dbContext.Stops.Take(rowsCount).Include(e => e.StopID).ToListAsync();
                if (entities != null)
                {
                    memoryCache.Set(cacheKey, entities,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CachingTime)));
                }
            }
            return entities;
        }
        public async Task DeleteRangeAsync(IEnumerable<Stop> entities)
        {
            dbContext.Set<Stop>().RemoveRange(entities);
            await Task.CompletedTask; // Или удалите эту строку, если не нужно.
        }

        public async Task DeleteRange(IEnumerable<Stop> entities)
        {
            dbContext.Set<Stop>().RemoveRange(entities);
            await dbContext.SaveChangesAsync(); // Сохраняем изменения в базе данных
        }
    }
}
