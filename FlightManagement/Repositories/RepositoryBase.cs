using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FlightManagement.DAL.Interfaces.Repositories;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Repositories
{
    //public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    //{
    //    protected AppDbContext dbContext;
    //    protected const int CachingTime = 16 * 2 + 240;

    //    public RepositoryBase(AppDbContext dbContext)
    //    {
    //        this.dbContext = dbContext;

    //    }

    //    public async Task CreateEntities(IEnumerable<T> entities)
    //    {
    //        await dbContext.AddRangeAsync(entities);
    //        await dbContext.SaveChangesAsync();
    //    }

    //    public async Task<int> CreateEntity(T entity)
    //    {
    //        await dbContext.Set<T>().AddAsync(entity);
    //        return await dbContext.SaveChangesAsync();
    //    }

    //    public async Task<int> DeleteEntity(T entity)
    //    {
    //        dbContext.Set<T>().Remove(entity);
    //        return await dbContext.SaveChangesAsync();
    //    }

    //    public IQueryable<T> GetAllEntities(bool trackChanges)
    //    {
    //        return !trackChanges ? dbContext.Set<T>().AsNoTracking() : dbContext.Set<T>();
    //    }

    //    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
    //    {
    //        return !trackChanges ?
    //            dbContext.Set<T>().Where(expression).AsNoTracking() :
    //            dbContext.Set<T>().Where(expression);
    //    }

    //    public async Task UpdateEntity(T entity)
    //    {
    //        dbContext.Set<T>().Update(entity);
    //        await dbContext.SaveChangesAsync();
    //    }
    //}
}
