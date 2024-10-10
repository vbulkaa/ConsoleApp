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

        public async Task<IEnumerable<FlightsDto>> GetAllFlights()
        {
            var flights = await _repositoryManager.FlightsRepository.GetAll(false);
            return _mapper.Map<IEnumerable<FlightsDto>>(flights);
        }

        public async Task<FlightsDto> GetFlightById(int id)
        {
            var flight = await _repositoryManager.FlightsRepository.GetById(id, false);
            return _mapper.Map<FlightsDto>(flight);
        }

        public async Task CreateFlight(FlightsForCreationDto flightForCreation)
        {
            var flight = _mapper.Map<Flights>(flightForCreation);
            await _repositoryManager.FlightsRepository.Create(flight);
        }

        public async Task UpdateFlight(int id, FlightsForUpdateDto flightForUpdate)
        {
            var flight = await _repositoryManager.FlightsRepository.GetById(id, true);
            if (flight == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            _mapper.Map(flightForUpdate, flight);
            await _repositoryManager.FlightsRepository.Update(flight);
        }

        public async Task DeleteFlight(int id)
        {
            var flight = await _repositoryManager.FlightsRepository.GetById(id, false);
            if (flight == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            await _repositoryManager.FlightsRepository.Delete(flight);
        }

        public async Task Create(IEnumerable<Flights> entityForCreation) =>
            await _repositoryManager.FlightsRepository.Create(entityForCreation);

        public async Task<IEnumerable<FlightsDto>> Get(int rowsCount, string cacheKey)
        {
            var flights = await _repositoryManager.FlightsRepository.Get(rowsCount, cacheKey);
            return _mapper.Map<IEnumerable<FlightsDto>>(flights);

        }

        public async Task Create(IEnumerable<FlightsDto> flightsForCreation)
        {
            var flights = _mapper.Map<IEnumerable<Flights>>(flightsForCreation);
            await _repositoryManager.FlightsRepository.Create(flights);
        }
    }
}
