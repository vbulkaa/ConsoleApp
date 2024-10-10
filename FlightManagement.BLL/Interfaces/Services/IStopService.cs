using FlightManagement.DTO.Stops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IStopService
    {
        Task<IEnumerable<StopsDto>> GetAllStops();
        Task<StopsDto> GetStopById(int id);
        Task CreateStop(StopsForCreationDto stopForCreation);
        Task UpdateStop(int id, StopsForUpdateDto stopForUpdate);
        Task DeleteStop(int id);
    }
}
