using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DTO.Statuses;
using FlightManagement.DTO.Stops;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Services
{
    public class StopService : IStopService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public StopService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<Stops> Create(StopsForCreationDto entityForCreation)
        {
            var entity = _mapper.Map<Stops>(entityForCreation);

            await _repositoryManager.StopsRepository.Create(entity);
            await _repositoryManager.SaveAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _repositoryManager.StopsRepository.GetById(id, trackChanges: false);

            if (entity == null)
            {
                return false;
            }

            _repositoryManager.StopsRepository.Delete(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Stops>> Get(int rowsCount, string cacheKey) =>
            await _repositoryManager.StopsRepository.Get(rowsCount, cacheKey);

        public async Task<IEnumerable<Stops>> GetAll() =>
            await _repositoryManager.StopsRepository.GetAll(false);

        public async Task<Stops> GetById(int id) =>
            await _repositoryManager.StopsRepository.GetById(id, false);

        public async Task<bool> Update(StopsForUpdateDto entityForUpdate)
        {
            var entity = await _repositoryManager.StopsRepository.GetById(entityForUpdate.StopID, trackChanges: true);

            if (entity == null)
            {
                return false;
            }

            _mapper.Map(entityForUpdate, entity);

            _repositoryManager.StopsRepository.Update(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}
