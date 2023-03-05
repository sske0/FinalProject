using Core.Entities;
using Core.Helpers;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class OwnerService
    {
        private readonly AdminRepository _adminRepository;
        private readonly OwnerRepository _ownerRepository;
        public OwnerService()
        {
            _adminRepository = new AdminRepository();
            _ownerRepository = new OwnerRepository();
        }
        public void GetAll()
        {
            if (_ownerRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No owners yet", ConsoleColor.Yellow);
            }
            var owners = _ownerRepository.GetAll();
            foreach (var owner in owners)
            {
                ConsoleHelper.WriteWithColor($"Id: {owner.Id} Name: {owner.Name} Surname: {owner.Surname}, Created by: {owner.CreatedBy}", ConsoleColor.Magenta);
            }
        }
        public void Create(Admin admin)
        {
            ConsoleHelper.WriteWithColor("Enter owner's name", ConsoleColor.Blue);
            string name = Console.ReadLine();

            ConsoleHelper.WriteWithColor("Enter owner's surname", ConsoleColor.Blue);
            string surname = Console.ReadLine();

            var owner = new Owner
            {
                Name = name,
                Surname = surname,
                CreatedBy= admin.Username,
                CreatedAt = DateTime.Now
            };

            _ownerRepository.Add(owner);
            ConsoleHelper.WriteWithColor($"Owner *{owner.Name} {owner.Surname}* was successfully added ", ConsoleColor.Green);
        }
        public void Update()
        {
            if (_ownerRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No owners yet", ConsoleColor.Yellow);
                return;
            }
            else
            {
                GetAll();
            OwnerIdInput: ConsoleHelper.WriteWithColor("Enter owner's id: ", ConsoleColor.Blue);
                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto OwnerIdInput;
                }

                var owner = _ownerRepository.Get(id);
                if (owner is null)
                {
                    ConsoleHelper.WriteWithColor("There is no owner with this id", ConsoleColor.Yellow);
                    goto OwnerIdInput;
                }
                ConsoleHelper.WriteWithColor("Enter a new name: ", ConsoleColor.Blue);
                string name = Console.ReadLine();

                ConsoleHelper.WriteWithColor("Enter a new surname: ", ConsoleColor.Blue);
                string surname = Console.ReadLine();

                owner.Name = name;
                owner.Surname = surname;

                _ownerRepository.Update(owner);

                ConsoleHelper.WriteWithColor($"{owner.Name} {owner.Surname} was updated", ConsoleColor.Green);
            }
        }
        public void Delete()
        {
            if (_ownerRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No owners yet", ConsoleColor.Yellow);
                return;
            }
            else
            {
                OwnerIdInput: GetAll();
                ConsoleHelper.WriteWithColor("Enter owner's id: ", ConsoleColor.Blue);
                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto OwnerIdInput;
                }

                var owner = _ownerRepository.Get(id);
                if (owner is null)
                {
                    ConsoleHelper.WriteWithColor("There is no owner with this id", ConsoleColor.Yellow);
                }

                _ownerRepository.Delete(owner);
                ConsoleHelper.WriteWithColor($"{owner.Name} {owner.Surname} was deleted", ConsoleColor.Green);
            }
        }
    }
}
