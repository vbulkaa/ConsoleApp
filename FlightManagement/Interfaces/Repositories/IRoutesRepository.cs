using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IRoutesRepository
    {
        Task<IEnumerable<Routes>> GetAll(bool trackChanges);
        IQueryable<Routes> GetAllEntities(bool trackChanges);
        Task<IEnumerable<Routes>> Get(int rowsCount, string cacheKey);
        Task<Routes> GetById(int id, bool trackChanges);
        Task Create(Routes entity);
        //Task Create(IEnumerable<Routes> entities);
        Task Delete(Routes entity);
        Task Update(Routes entity);
    }
}
