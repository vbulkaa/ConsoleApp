using FlightManagement.DTO.Rotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Interfaces.Services
{
    public interface IRouteService
    {
        Task<IEnumerable<RoutesDto>> GetAllRoutes();
        Task<RoutesDto> GetRouteById(int id);
        Task CreateRoute(RoutesForCreationDto routeForCreation);
        Task UpdateRoute(int id, RoutesForUpdateDto routeForUpdate);
        Task DeleteRoute(int id);
    }
}
