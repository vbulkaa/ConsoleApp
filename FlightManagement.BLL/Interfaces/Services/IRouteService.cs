using FlightManagement.DTO.Flights;
using FlightManagement.DTO.Rotes;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IRouteService
    {
        Task<IEnumerable<Routes>> GetAll();
       
        Task<IEnumerable<Routes>> Get(int rowsCount, string cacheKey);
        Task<Routes> GetById(int id);
        Task<Routes> Create(RoutesForCreationDto routeForCreation);
        Task<bool> Update(RoutesForUpdateDto routeForUpdate);
        Task<bool> Delete(int id);
    }
}
