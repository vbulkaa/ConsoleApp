using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightManagement.DTO.Airport;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IAirportService
    {
        Task<IEnumerable<AirportsDto>> GetAllAirports();
        Task<AirportsDto> GetAirportById(int id);
        Task CreateAirport(AirportsForCreationDto airportForCreation);
        Task UpdateAirport(int id, AirportsForUpdateDto airportForUpdate);
        Task DeleteAirport(int id);
    }
}
