using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Rotes;
using FlightManagement.DTO.Statuses;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Services
{
    public class StatusService : IStatusService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public StatusService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<Statuses> Create(StatusesForCreationDto entityForCreation)
        {
            var entity = _mapper.Map<Statuses>(entityForCreation);

            await _repositoryManager.StatusesRepository.Create(entity);
            await _repositoryManager.SaveAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _repositoryManager.StatusesRepository.GetById(id, trackChanges: false);

            if (entity == null)
            {
                return false;
            }

            _repositoryManager.StatusesRepository.Delete(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Statuses>> Get(int rowsCount, string cacheKey) =>
            await _repositoryManager.StatusesRepository.Get(rowsCount, cacheKey);

        public async Task<IEnumerable<Statuses>> GetAll() =>
            await _repositoryManager.StatusesRepository.GetAll(false);

        public async Task<Statuses> GetById(int id) =>
            await _repositoryManager.StatusesRepository.GetById(id, false);

        public async Task<bool> Update(StatusesForUpdateDto entityForUpdate)
        {
            var entity = await _repositoryManager.StatusesRepository.GetById(entityForUpdate.StatusID, trackChanges: true);

            if (entity == null)
            {
                return false;
            }

            _mapper.Map(entityForUpdate, entity);

            _repositoryManager.StatusesRepository.Update(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}
