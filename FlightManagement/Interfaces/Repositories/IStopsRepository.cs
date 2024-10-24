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
        Task<IEnumerable<Stops>> GetAll(bool trackChanges);
        Task<IEnumerable<Stops>> Get(int rowsCount, string cacheKey);
        IQueryable<Stops> GetAllEntities(bool trackChanges);
        IQueryable<Stops> GetByCondition(Expression<Func<Stops, bool>> expression, bool trackChanges);
        Task<Stops> GetById(int id, bool trackChanges);
        Task DeleteRangeAsync(IEnumerable<Stops> entities);
        Task Create(Stops entity);
        //Task Create(IEnumerable<Stops> entities);
        Task Delete(Stops entity);
        Task Update(Stops entity);
    }
}
