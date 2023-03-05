using Core.Entities;
using Core.Helpers;
using Data.Contexts;
using Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete
{
    public class AdminRepository : IAdminRepository
    {
        public Admin GetByUsernameAndPassword(string username, string password)
        {
            return DbContext.Admins.FirstOrDefault(a => a.Username == username.ToLower() && PasswordHasher.Decrypt(a.Password) == password);
        }
    }
}
