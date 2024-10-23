using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IStatusesRepository
    {
        Task<IEnumerable<Statuses>> GetAll(bool trackChanges);
        Task<IEnumerable<Statuses>> Get(int rowsCount, string cacheKey);
        Task<Statuses> GetById(int id, bool trackChanges);
        Task Create(Statuses entity);
       // Task Create(IEnumerable<Statuses> entities);
        Task Delete(Statuses entity);
        Task Update(Statuses entity);
    }
}
