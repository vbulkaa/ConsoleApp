using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Statuses;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAll();
        Task<IEnumerable<Status>> Get(int rowsCount, string cacheKey);
        Task<Status> GetById(int id);
        Task<Status> Create(StatusesForCreationDto statusForCreation);
        Task<bool> Update(StatusesForUpdateDto statusForUpdate);
        Task<bool> Delete(int id);
    }
}
