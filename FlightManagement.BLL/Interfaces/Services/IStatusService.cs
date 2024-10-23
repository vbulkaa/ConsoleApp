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
        Task<IEnumerable<Statuses>> GetAll();
        Task<IEnumerable<Statuses>> Get(int rowsCount, string cacheKey);
        Task<Statuses> GetById(int id);
        Task<Statuses> Create(StatusesForCreationDto statusForCreation);
        Task<bool> Update(StatusesForUpdateDto statusForUpdate);
        Task<bool> Delete(int id);
    }
}
