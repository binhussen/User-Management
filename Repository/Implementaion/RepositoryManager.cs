using Contracts.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementaion
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _repositoryContext;
        private IUserRepository _userRepository;
        public RepositoryManager(RepositoryDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_repositoryContext);

                return _userRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}
