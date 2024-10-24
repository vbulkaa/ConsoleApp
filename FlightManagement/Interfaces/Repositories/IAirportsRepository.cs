using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IAirportsRepository
    {
        IQueryable<Airports> GetAllEntities(bool trackChanges);
        Task<IEnumerable<Airports>> GetAll(bool trackChanges);
        Task<IEnumerable<Airports>> Get(int rowsCount, string cacheKey);
        Task<Airports> GetById(int id, bool trackChanges);
        Task Create(Airports entity);
        //Task Create(IEnumerable<Airports> entities);
        Task Delete(Airports entity);
        Task Update(Airports entity);
    }
}
