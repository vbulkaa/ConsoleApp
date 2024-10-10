using FlightManagement.DTO.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IStatusService
    {
        Task<IEnumerable<StatusesDto>> GetAllStatuses();
        Task<StatusesDto> GetStatusById(int id);
        Task CreateStatus(StatusesForCreationDto statusForCreation);
        Task UpdateStatus(int id, StatusesForUpdateDto statusForUpdate);
        Task DeleteStatus(int id);
    }
}
