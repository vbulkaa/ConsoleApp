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
        Task<IEnumerable<Stop>> GetAll();
        Task<IEnumerable<Stop>> Get(int rowsCount, string cacheKey);
        Task<Stop> GetById(int id);
        Task<Stop> Create(StopsForCreationDto stopForCreation);
        Task<bool> Update(StopsForUpdateDto stopForUpdate);
        Task<bool> Delete(int id);
    }
}
