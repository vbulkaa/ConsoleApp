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
        Task<IEnumerable<Flight>> GetAll();
        Task<IEnumerable<Flight>> Get(int rowsCount, string cacheKey);
        Task<Flight> GetById(int id);
        Task<Flight> Create(FlightsForCreationDto flightForCreationDto);
        Task<bool> Update(FlightsForUpdateDto flightForUpdateDto);
        Task<bool> Delete(int id);
    }
}
