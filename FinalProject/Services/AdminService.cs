using Core.Entities;
using Core.Helpers;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepository;
        public AdminService()
        {
            _adminRepository = new AdminRepository();
        }
        public Admin Authorize()
        {
        Login: ConsoleHelper.WriteWithColor("--- Login ---", ConsoleColor.Cyan);

            ConsoleHelper.WriteWithColor("Enter username: ", ConsoleColor.Cyan);
            string username = Console.ReadLine();

            ConsoleHelper.WriteWithColor("Enter password: ", ConsoleColor.Cyan);
            string password = Console.ReadLine();

            var admin = _adminRepository.GetByUsernameAndPassword(username, password);
            if (admin is null)
            {
                ConsoleHelper.WriteWithColor("Username or password is incorrect", ConsoleColor.Yellow);
                goto Login;
            }
            return admin;
        }
    }
}
