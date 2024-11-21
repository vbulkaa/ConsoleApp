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
using FlightManagement.DTO.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.BLL.Services
{

    public class AirportService : IAirportService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
       // private readonly ILogger<AirportService> _logger;

        public AirportService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
           
            _mapper = mapper;
        }


        public async Task<Airport> Create(AirportsForCreationDto entityForCreation)
        {
            if (entityForCreation == null)
            {
                throw new ArgumentNullException(nameof(entityForCreation), "Данные для создания аэропорта не могут быть null.");
            }

            // Маппинг
            var entity = _mapper.Map<Airport>(entityForCreation);

            // Обработка создания в репозитории
            try
            {
                //_logger.LogInformation("Создание аэропорта: {@Airport}", entity);
                await _repositoryManager.AirportsRepository.Create(entity);
                await _repositoryManager.SaveAsync();
            }
            catch (DbUpdateException ex) // или другой тип исключения
            {
                // Логирование ошибки
                //_logger.LogError(ex, "Ошибка при добавлении аэропорта: {AirportName}", entityForCreation.Name);
                throw new Exception("Не удалось создать аэропорт. Пожалуйста, проверьте данные и попробуйте снова.");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Произошла ошибка при создании аэропорта");
                throw; // Перекиньте исключение дальше
            }

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _repositoryManager.AirportsRepository.GetById(id, trackChanges: false);

            if (entity == null)
            {
                return false;
            }

            _repositoryManager.AirportsRepository.Delete(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Airport>> Get(int rowsCount, string cacheKey) =>
            await _repositoryManager.AirportsRepository.Get(rowsCount, cacheKey);

        public async Task<IEnumerable<Airport>> GetAll() =>
            await _repositoryManager.AirportsRepository.GetAll(false);

        public async Task<Airport> GetById(int id) =>
            await _repositoryManager.AirportsRepository.GetById(id, false);

        public async Task<bool> Update(AirportsForUpdateDto entityForUpdate)
        {
            var entity = await _repositoryManager.AirportsRepository.GetById(entityForUpdate.AirportID, trackChanges: true);

            if (entity == null)
            {
                return false;
            }

            _mapper.Map(entityForUpdate, entity);

            _repositoryManager.AirportsRepository.Update(entity);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}
