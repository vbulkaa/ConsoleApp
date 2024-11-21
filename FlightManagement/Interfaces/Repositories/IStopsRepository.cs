using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IStopsRepository
    {
        Task<IEnumerable<Stop>> GetAll(bool trackChanges);
        Task<IEnumerable<Stop>> Get(int rowsCount, string cacheKey);
        IQueryable<Stop> GetAllEntities(bool trackChanges);
        IQueryable<Stop> GetByCondition(Expression<Func<Stop, bool>> expression, bool trackChanges);
        Task<Stop> GetById(int id, bool trackChanges);
        Task DeleteRangeAsync(IEnumerable<Stop> entities);
       
        Task Create(Stop entity);
        //Task Create(IEnumerable<Stops> entities);
        Task Delete(Stop entity);
        Task Update(Stop entity);
        Task DeleteRange(IEnumerable<Stop> entities);
    }
}
