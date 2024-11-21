using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightManagement.DAL.Repositories.Base;
using Microsoft.Extensions.Caching.Memory;

namespace FlightManagement.DAL.Repositories
{
    public class StatusesRepository : RepositoryBase<Status>, IStatusesRepository
    {
        private readonly AppDbContext _dbContext;
        public StatusesRepository(AppDbContext context, IMemoryCache memoryCache) : base(context, memoryCache) 
        {
            _dbContext = dbContext;
        }


        //public StatusesRepository(AppDbContext dbContext)
        //    : base(dbContext)
        //{
        //}
        //public IQueryable<Statuses> GetAllEntities(bool trackChanges)
        //{
        //    return !trackChanges ?
        //        _dbContext.Statuses.AsNoTracking() :
        //        _dbContext.Statuses;
        //}
        public async Task Create(Status entity) =>
            await CreateEntity(entity);

        public async Task Delete(Status entity) =>
            await Delete(entity);

        public async Task<IEnumerable<Status>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Status> GetById(int id, bool trackChanges) =>
            await GetByCondition(s => s.StatusID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Status entity) =>
            await Update(entity);

        public async Task<IEnumerable<Status>> Get(int rowsCount, string cacheKey)
        {
            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Status> entities))
            {
                entities = await dbContext.Statuses.Take(rowsCount).Include(e => e.StatusID).ToListAsync();
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
