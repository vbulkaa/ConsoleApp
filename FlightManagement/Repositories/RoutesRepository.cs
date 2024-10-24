
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using FlightManagement.DAL.Repositories.Base;
using FlightManagement.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace FlightManagement.DAL.Repositories
{
    public class RoutesRepository : RepositoryBase<Routes>, IRoutesRepository
    {
        public RoutesRepository(AppDbContext context, IMemoryCache memoryCache) : base(context, memoryCache) { }

        public async Task<Routes> GetByIdAsync(int id, bool trackChanges)
        {
            return await GetByCondition(r => r.RouteID == id, trackChanges).SingleOrDefaultAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Routes> entities)
        {
            dbContext.Set<Routes>().RemoveRange(entities);
            await Task.CompletedTask; // Или удалите эту строку, если не нужно.
        }
        public async Task Create(Routes entity) =>
            await CreateEntity(entity);


        public async Task Delete(Routes entity) =>
            await Delete(entity);

        public async Task<IEnumerable<Routes>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Routes> GetById(int id, bool trackChanges) =>
            await GetByCondition(r => r.RouteID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Routes entity) =>
            await Update(entity);

        public async Task<IEnumerable<Routes>> Get(int rowsCount, string cacheKey)
        {
            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Routes> entities))
            {
                entities = await dbContext.Routes.Take(rowsCount).Include(e => e.RouteID).ToListAsync();
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

