using AutoMapper;
using FlightManagement.BLL.Interfaces.Services;
using FlightManagement.DTO.Airport;
using FlightManagement.DAL.Interfaces;
using FlightManagement.DAL.models;
using FlightManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.BLL.Services
{
    public class AirportService : IAirportService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public AirportService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AirportsDto>> GetAllAirports()
        {
            var airports = await _repositoryManager.AirportsRepository.GetAll(false);
            return _mapper.Map<IEnumerable<AirportsDto>>(airports);
        }

        public async Task<AirportsDto> GetAirportById(int id)
        {
            var airport = await _repositoryManager.AirportsRepository.GetById(id, false);
            return _mapper.Map<AirportsDto>(airport);
        }

        public async Task CreateAirport(AirportsForCreationDto airportForCreation)
        {
            var airport = _mapper.Map<Airports>(airportForCreation);
            await _repositoryManager.AirportsRepository.Create(airport);
        }

        public async Task UpdateAirport(int id, AirportsForUpdateDto airportForUpdate)
        {
            var airport = await _repositoryManager.AirportsRepository.GetById(id, true);
            if (airport == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            _mapper.Map(airportForUpdate, airport);
            await _repositoryManager.AirportsRepository.Update(airport);
        }

        public async Task DeleteAirport(int id)
        {
            var airport = await _repositoryManager.AirportsRepository.GetById(id, false);
            if (airport == null)
            {
                throw new Exception($"Entity with id {id} doesn't exist in database!");
            }
            await _repositoryManager.AirportsRepository.Delete(airport);
        }
    }
}
