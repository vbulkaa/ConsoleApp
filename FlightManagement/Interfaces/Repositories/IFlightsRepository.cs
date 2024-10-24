using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IFlightsRepository
    {
        Task<IEnumerable<Flights>> GetAll(bool trackChanges);
        IQueryable<Flights> GetAllEntities(bool trackChanges);
        Task<IEnumerable<Flights>> Get(int rowsCount, string cacheKey);
        //Task<Flights> GetByIdAsync(int flightId);
        Task<Flights> GetById(int id, bool trackChanges);
        Task Create(Flights entity);
       // Task Create(IEnumerable<Flights> entities);
        Task Delete(Flights entity);
        Task Update(Flights entity);
       
    }
}
