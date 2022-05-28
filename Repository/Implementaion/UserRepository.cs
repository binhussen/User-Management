using Contracts.Interfaces;
using Entities;
using Entities.Models;
using Entities.Parameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementaion
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryDbContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateUser(User user)
        {
            Create(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public async Task<User> GetUserAsync(int id, bool trackChanges) =>
         await FindByCondition(e => e.id.Equals(id) && e.id.Equals(id), trackChanges)
             .SingleOrDefaultAsync();

        public async Task<PagedList<User>> GetUsersAsync( RequestParameters requestParameters, bool trackChanges)
        {
            var users = await FindAll(trackChanges)
                    .OrderBy(c => c.First_Name)
                    .ToListAsync();
            return PagedList<User>
                .ToPagedList(users, requestParameters.PageNumber, requestParameters.PageSize);
        }
    }
}
