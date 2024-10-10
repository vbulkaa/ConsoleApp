using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Repositories
{
    public class AirportsRepository : RepositoryBase<Airports>, IAirportsRepository
    {
        private readonly IMemoryCache _memoryCache;

        public AirportsRepository(AppDbContext dbContext, IMemoryCache memoryCache)
            : base(dbContext)
        {
            _memoryCache = memoryCache;
        }

        public async Task Create(Airports entity) =>
            await CreateEntity(entity);

        public async Task Create(IEnumerable<Airports> entities) =>
            await CreateEntities(entities);

        public async Task Delete(Airports entity) =>
            await DeleteEntity(entity);

        public async Task<IEnumerable<Airports>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Airports> GetById(int id, bool trackChanges) =>
            await GetByCondition(a => a.AirportID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Airports entity) =>
            await UpdateEntity(entity);
    }
}
