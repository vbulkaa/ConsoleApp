using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Repositories
{
    public class StopsRepository : RepositoryBase<Stops>, IStopsRepository
    {
        public StopsRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task Create(Stops entity) =>
            await CreateEntity(entity);

        public async Task Create(IEnumerable<Stops> entities) =>
            await CreateEntities(entities);

        public async Task Delete(Stops entity) =>
            await DeleteEntity(entity);

        public async Task<IEnumerable<Stops>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Stops> GetById(int id, bool trackChanges) =>
            await GetByCondition(s => s.StopID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Stops entity) =>
            await UpdateEntity(entity);
    }
}
