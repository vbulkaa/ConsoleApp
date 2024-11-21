using FlightManagement.models;
using Microsoft.AspNetCore.Routing;
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
        Task<IEnumerable<FlightManagement.models.Route>> GetAll(bool trackChanges);
        IQueryable<FlightManagement.models.Route> GetAllEntities(bool trackChanges);
        Task<IEnumerable<FlightManagement.models.Route>> Get(int rowsCount, string cacheKey);
        Task<FlightManagement.models.Route> GetByIdAsync(int id, bool trackChanges);
        //  Task<Routes> GetByIdAsync(int flightId);
        Task<FlightManagement.models.Route> GetById(int id, bool trackChanges);
        Task DeleteRangeAsync(IEnumerable<FlightManagement.models.Route> entities);
        Task Create(FlightManagement.models.Route entity);
        IQueryable<FlightManagement.models.Route> GetByCondition(Expression<Func<FlightManagement.models.Route, bool>> expression, bool trackChanges);
        //Task Create(IEnumerable<Routes> entities);
        Task Delete(FlightManagement.models.Route entity);
        Task Update(FlightManagement.models.Route entity);
        Task DeleteRange(IEnumerable<FlightManagement.models.Route> entities);
    }
}
