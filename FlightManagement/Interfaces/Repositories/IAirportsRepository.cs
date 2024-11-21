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
        IQueryable<Airport> GetAllEntities(bool trackChanges);
        Task<IEnumerable<Airport>> GetAll(bool trackChanges);
        Task<IEnumerable<Airport>> Get(int rowsCount, string cacheKey);
        Task<Airport> GetById(int id, bool trackChanges);
        Task Create(Airport entity);
        //Task Create(IEnumerable<Airports> entities);
        Task Delete(Airport entity);
        Task Update(Airport entity);
    }
}
