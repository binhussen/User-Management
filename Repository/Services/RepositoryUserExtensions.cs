using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services
{
    public static class RepositoryUserExtensions
    {
        public static IQueryable<User> Search(this IQueryable<User> users,string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return users;
            var lowerCaseTerm = firstName.Trim().ToLower();
            return users.Where(e => e.First_Name.ToLower().Contains(lowerCaseTerm));
        }
    }
}
