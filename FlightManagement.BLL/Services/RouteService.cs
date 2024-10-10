using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Rotes;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public RouteService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoutesDto>> GetAllRoutes()
        {
            var routes = await _repositoryManager.RoutesRepository.GetAll(false);
            return _mapper.Map<IEnumerable<RoutesDto>>(routes);
        }

        public async Task<RoutesDto> GetRouteById(int id)
        {
            var route = await _repositoryManager.RoutesRepository.GetById(id, false);
            return _mapper.Map<RoutesDto>(route);
        }

        public async Task CreateRoute(RoutesForCreationDto routeForCreation)
        {
            var route = _mapper.Map<Routes>(routeForCreation);
            await _repositoryManager.RoutesRepository.Create(route);
        }

        public async Task UpdateRoute(int id, RoutesForUpdateDto routeForUpdate)
        {
            var route = await _repositoryManager.RoutesRepository.GetById(id, true);
            if (route == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            _mapper.Map(routeForUpdate, route);
            await _repositoryManager.RoutesRepository.Update(route);
        }

        public async Task DeleteRoute(int id)
        {
            var route = await _repositoryManager.RoutesRepository.GetById(id, false);
            if (route == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            await _repositoryManager.RoutesRepository.Delete(route);
        }
    }
}
