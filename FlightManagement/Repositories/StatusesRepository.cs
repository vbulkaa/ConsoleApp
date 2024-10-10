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
    public class StatusesRepository : RepositoryBase<Statuses>, IStatusesRepository
    {
        public StatusesRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task Create(Statuses entity) =>
            await CreateEntity(entity);

        public async Task Create(IEnumerable<Statuses> entities) =>
            await CreateEntities(entities);

        public async Task Delete(Statuses entity) =>
            await DeleteEntity(entity);

        public async Task<IEnumerable<Statuses>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Statuses> GetById(int id, bool trackChanges) =>
            await GetByCondition(s => s.StatusID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Statuses entity) =>
            await UpdateEntity(entity);
    }
}
