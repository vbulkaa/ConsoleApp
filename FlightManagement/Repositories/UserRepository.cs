using FlightManagement.DAL.Interfaces.Repositories;
using FlightManagement.DAL.models.Users;
using FlightManagement.DAL.models.People;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace FlightManagement.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAllEntities(bool trackChanges)
        {
            return trackChanges ? _context.Users : _context.Users.AsNoTracking();
        }

        public IQueryable<User> GetByCondition(Expression<Func<User, bool>> expression, bool trackChanges)
        {
            return trackChanges ? _context.Users.Where(expression) : _context.Users.Where(expression).AsNoTracking();
        }

        public async Task CreateEntity(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public void UpdateEntity(User entity)
        {
            _context.Users.Update(entity);
        }

        public void DeleteEntity(User entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IList<User>> GetAllRegularUsersAsync()
        {
            return await _context.Users.OfType<RegularUser>().Cast<User>().ToListAsync();
        }

        public async Task<IList<User>> GetAllAdminUsersAsync()
        {
            return await _context.Users.OfType<AdminUser>().Cast<User>().ToListAsync();
        }

        Task<int> IRepositoryBase<User>.CreateEntity(User entity)
        {
            throw new NotImplementedException();
        }

        public Task CreateEntities(IEnumerable<User> entities)
        {
            throw new NotImplementedException();
        }

        Task IRepositoryBase<User>.UpdateEntity(User entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepositoryBase<User>.DeleteEntity(User entity)
        {
            throw new NotImplementedException();
        }
    }

}
