using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Flights;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Services
{
    public class FlightService : IFlightService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public FlightService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<Flights> Create(FlightsForCreationDto entityForCreation)
        {
            var entity = _mapper.Map<Flights>(entityForCreation);

            await _repositoryManager.FlightsRepository.Create(entity);
            await _repositoryManager.SaveAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _repositoryManager.FlightsRepository.GetById(id, trackChanges: false);

            if (entity == null)
            {
                return false;
            }

            _repositoryManager.FlightsRepository.Delete(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Flights>> Get(int rowsCount, string cacheKey) =>
            await _repositoryManager.FlightsRepository.Get(rowsCount, cacheKey);

        public async Task<IEnumerable<Flights>> GetAll() =>
            await _repositoryManager.FlightsRepository.GetAll(false);

        public async Task<Flights> GetById(int id) =>
            await _repositoryManager.FlightsRepository.GetById(id, false);

        public async Task<bool> Update(FlightsForUpdateDto entityForUpdate)
        {
            var entity = await _repositoryManager.FlightsRepository.GetById(entityForUpdate.FlightID, trackChanges: true);

            if (entity == null)
            {
                return false;
            }

            _mapper.Map(entityForUpdate, entity);

            _repositoryManager.FlightsRepository.Update(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}
