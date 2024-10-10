using FlightManagement.DTO.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightsDto>> GetAllFlights();
        Task<IEnumerable<FlightsDto>> Get(int rowsCount, string cacheKey);
        Task<FlightsDto> GetFlightById(int id);
        Task CreateFlight(FlightsForCreationDto flightForCreation);
        Task Create(IEnumerable<FlightsDto> flights);
        Task UpdateFlight(int id, FlightsForUpdateDto flightForUpdate);
        Task DeleteFlight(int id);
    }
}
