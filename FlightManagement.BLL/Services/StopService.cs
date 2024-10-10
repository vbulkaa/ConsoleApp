using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DAL.Interfaces;
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

        public async Task<IEnumerable<StopsDto>> GetAllStops()
        {
            var stops = await _repositoryManager.StopsRepository.GetAll(false);
            return _mapper.Map<IEnumerable<StopsDto>>(stops);
        }

        public async Task<StopsDto> GetStopById(int id)
        {
            var stop = await _repositoryManager.StopsRepository.GetById(id, false);
            return _mapper.Map<StopsDto>(stop);
        }

        public async Task CreateStop(StopsForCreationDto stopForCreation)
        {
            var stop = _mapper.Map<Stops>(stopForCreation);
            await _repositoryManager.StopsRepository.Create(stop);
        }

        public async Task UpdateStop(int id, StopsForUpdateDto stopForUpdate)
        {
            var stop = await _repositoryManager.StopsRepository.GetById(id, true);
            if (stop == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            _mapper.Map(stopForUpdate, stop);
            await _repositoryManager.StopsRepository.Update(stop);
        }

        public async Task DeleteStop(int id)
        {
            var stop = await _repositoryManager.StopsRepository.GetById(id, false);
            if (stop == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            await _repositoryManager.StopsRepository.Delete(stop);
        }
    }
}
