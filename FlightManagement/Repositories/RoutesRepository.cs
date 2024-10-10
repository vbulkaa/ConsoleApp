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
    public class RoutesRepository : RepositoryBase<Routes>, IRoutesRepository
    {
        public RoutesRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task Create(Routes entity) =>
            await CreateEntity(entity);

        public async Task Create(IEnumerable<Routes> entities) =>
            await CreateEntities(entities);

        public async Task Delete(Routes entity) =>
            await DeleteEntity(entity);

        public async Task<IEnumerable<Routes>> GetAll(bool trackChanges) =>
            await GetAllEntities(trackChanges).ToListAsync();

        public async Task<Routes> GetById(int id, bool trackChanges) =>
            await GetByCondition(r => r.RouteID.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task Update(Routes entity) =>
            await UpdateEntity(entity);
    }
}
