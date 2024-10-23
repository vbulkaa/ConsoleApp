using FlightManagement.DAL.models.Users;
using System;
using FlightManagement.DAL.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(string id); //Guid??
        Task<IList<User>> GetAllRegularUsersAsync();
        Task<IList<User>> GetAllAdminUsersAsync();
    }
}
