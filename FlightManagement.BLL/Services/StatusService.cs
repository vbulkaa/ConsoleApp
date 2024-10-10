using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL.Interfaces;
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

        public async Task<IEnumerable<StatusesDto>> GetAllStatuses()
        {
            var statuses = await _repositoryManager.StatusesRepository.GetAll(false);
            return _mapper.Map<IEnumerable<StatusesDto>>(statuses);
        }

        public async Task<StatusesDto> GetStatusById(int id)
        {
            var status = await _repositoryManager.StatusesRepository.GetById(id, false);
            return _mapper.Map<StatusesDto>(status);
        }

        public async Task CreateStatus(StatusesForCreationDto statusForCreation)
        {
            var status = _mapper.Map<Statuses>(statusForCreation);
            await _repositoryManager.StatusesRepository.Create(status);
        }

        public async Task UpdateStatus(int id, StatusesForUpdateDto statusForUpdate)
        {
            var status = await _repositoryManager.StatusesRepository.GetById(id, true);
            if (status == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            _mapper.Map(statusForUpdate, status);
            await _repositoryManager.StatusesRepository.Update(status);
        }

        public async Task DeleteStatus(int id)
        {
            var status = await _repositoryManager.StatusesRepository.GetById(id, false);
            if (status == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            await _repositoryManager.StatusesRepository.Delete(status);
        }
    }
}
