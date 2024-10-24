using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IRoutesRepository
    {
        Task<IEnumerable<Routes>> GetAll(bool trackChanges);
        IQueryable<Routes> GetAllEntities(bool trackChanges);
        Task<IEnumerable<Routes>> Get(int rowsCount, string cacheKey);
        Task<Routes> GetByIdAsync(int id, bool trackChanges);
        //  Task<Routes> GetByIdAsync(int flightId);
        Task<Routes> GetById(int id, bool trackChanges);
        Task DeleteRangeAsync(IEnumerable<Routes> entities);
        Task Create(Routes entity);
        IQueryable<Routes> GetByCondition(Expression<Func<Routes, bool>> expression, bool trackChanges);
        //Task Create(IEnumerable<Routes> entities);
        Task Delete(Routes entity);
        Task Update(Routes entity);
    }
}
