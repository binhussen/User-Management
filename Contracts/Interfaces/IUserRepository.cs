using Entities.Models;
using Entities.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetUsersAsync(UserParameters userParameters, bool trackChanges);
        Task<User> GetUserAsync(int id, bool trackChanges);
        void CreateUser(User user);
        void DeleteUser(User user);
    }
}
