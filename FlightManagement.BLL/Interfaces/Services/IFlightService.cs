using FlightManagement.DTO.Airport;
using FlightManagement.DTO.Flights;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flights>> GetAll();
        Task<IEnumerable<Flights>> Get(int rowsCount, string cacheKey);
        Task<Flights> GetById(int id);
        Task<Flights> Create(FlightsForCreationDto flightForCreationDto);
        Task<bool> Update(FlightsForUpdateDto flightForUpdateDto);
        Task<bool> Delete(int id);
    }
}
