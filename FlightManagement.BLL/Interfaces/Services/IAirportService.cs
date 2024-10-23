using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightManagement.models;
using FlightManagement.DTO.Airport;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IAirportService
    {

        Task<IEnumerable<Airports>> GetAll();
        Task<IEnumerable<Airports>> Get(int rowsCount, string cacheKey);
        Task<Airports> GetById(int id);
        Task<Airports> Create(AirportsForCreationDto airportForCreation);
        Task<bool> Update(AirportsForUpdateDto airportForUpdate);
        Task<bool> Delete(int id);
    }
}
