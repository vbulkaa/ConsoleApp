using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using FlightManagement.DAL.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Repositories.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext dbContext;
        protected readonly IMemoryCache memoryCache;
        protected const int CachingTime = 240;

        public RepositoryBase(AppDbContext dbContext,
            IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
        }

        public async Task CreateEntity(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public void DeleteEntity(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAllEntities(bool trackChanges)
        {
            return !trackChanges ? dbContext.Set<T>().AsNoTracking() : dbContext.Set<T>();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ?
                dbContext.Set<T>().Where(expression).AsNoTracking() :
                dbContext.Set<T>().Where(expression);
        }

        public void UpdateEntity(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }
    }
}
