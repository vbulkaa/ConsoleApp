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
        Task<IEnumerable<Flight>> GetAll(bool trackChanges);
        IQueryable<Flight> GetAllEntities(bool trackChanges);
        Task<IEnumerable<Flight>> Get(int rowsCount, string cacheKey);
        //Task<Flights> GetByIdAsync(int flightId);
        Task<Flight> GetById(int id, bool trackChanges);
        Task Create(Flight entity);
       // Task Create(IEnumerable<Flights> entities);
        Task Delete(Flight entity);
        Task Update(Flight entity);
       
    }
}
