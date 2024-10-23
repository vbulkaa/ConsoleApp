using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IStopsRepository
    {
        Task<IEnumerable<Stops>> GetAll(bool trackChanges);
        Task<IEnumerable<Stops>> Get(int rowsCount, string cacheKey);
        Task<Stops> GetById(int id, bool trackChanges);
        Task Create(Stops entity);
        //Task Create(IEnumerable<Stops> entities);
        Task Delete(Stops entity);
        Task Update(Stops entity);
    }
}
