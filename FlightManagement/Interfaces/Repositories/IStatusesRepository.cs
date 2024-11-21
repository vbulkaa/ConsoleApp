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
        Task<IEnumerable<Status>> GetAll(bool trackChanges);
        Task<IEnumerable<Status>> Get(int rowsCount, string cacheKey);
        Task<Status> GetById(int id, bool trackChanges);
        Task Create(Status entity);
       // Task Create(IEnumerable<Statuses> entities);
        Task Delete(Status entity);
        Task Update(Status entity);
    }
}
