using FlightManagement.DTO.Statuses;
using FlightManagement.DTO.Stops;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IStopService
    {
        Task<IEnumerable<Stops>> GetAll();
        Task<IEnumerable<Stops>> Get(int rowsCount, string cacheKey);
        Task<Stops> GetById(int id);
        Task<Stops> Create(StopsForCreationDto stopForCreation);
        Task<bool> Update(StopsForUpdateDto stopForUpdate);
        Task<bool> Delete(int id);
    }
}
