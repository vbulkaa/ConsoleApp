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
    public class StatusesRepository : RepositoryBase<Statuses>, IStatusesRepository
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
        public async Task Create(Statuses entity) =>
            await CreateEntity(entity);

        public async Task Delete(Statuses entity) =>
            await Delete(entity);

        public async Task<IEnumerable<Statuses>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Statuses> GetById(int id, bool trackChanges) =>
            await GetByCondition(s => s.StatusID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Statuses entity) =>
            await Update(entity);

        public async Task<IEnumerable<Statuses>> Get(int rowsCount, string cacheKey)
        {
            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Statuses> entities))
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
