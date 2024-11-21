using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Airport;
using FlightManagement.DTO.Rotes;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<Route> Create(RoutesForCreationDto entityForCreation)
        {
            var entity = _mapper.Map<Route>(entityForCreation);

            await _repositoryManager.RoutesRepository.Create(entity);
            await _repositoryManager.SaveAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _repositoryManager.RoutesRepository.GetById(id, trackChanges: false);

            if (entity == null)
            {
                return false;
            }

            _repositoryManager.RoutesRepository.Delete(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Route>> Get(int rowsCount, string cacheKey) =>
            await _repositoryManager.RoutesRepository.Get(rowsCount, cacheKey);

        public async Task<IEnumerable<Route>> GetAll() =>
            await _repositoryManager.RoutesRepository.GetAll(false);

        public async Task<Route> GetById(int id) =>
            await _repositoryManager.RoutesRepository.GetById(id, false);

        public async Task<bool> Update(RoutesForUpdateDto entityForUpdate)
        {
            var entity = await _repositoryManager.RoutesRepository.GetById(entityForUpdate.RouteID, trackChanges: true);

            if (entity == null)
            {
                return false;
            }

            _mapper.Map(entityForUpdate, entity);

            _repositoryManager.RoutesRepository.Update(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}
