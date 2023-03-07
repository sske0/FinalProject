using Core.Entities;
using Core.Extensions;
using Core.Helpers;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinalProject.Services
{
    public class DrugstoreService
    {
        private readonly DrugstoreRepository _drugstoreRepository;
        private readonly OwnerRepository _ownerRepository;
        private readonly DrugRepository _drugRepository;
        private readonly DruggistRepository _druggistRepository;


        public DrugstoreService()
        {
            _drugstoreRepository = new DrugstoreRepository();
            _ownerRepository = new OwnerRepository();
            _drugRepository = new DrugRepository();
            _druggistRepository = new DruggistRepository();
        }
        public void GetAll()
        {
            var drugstores = _drugstoreRepository.GetAll();
            foreach (var drugstore in drugstores)
            {
                ConsoleHelper.WriteWithColor($"Id: {drugstore.Id} Name: {drugstore.Name} Contact Number: {drugstore.ContactNumber} Address: {drugstore.Address} Email: {drugstore.Email}, Created by: {drugstore.CreatedBy}", ConsoleColor.Magenta);
            }
        }
        public void GetAllDrugstoresByOwner()
        {
            if (_drugstoreRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("A drugstore must be created beforehand", ConsoleColor.Yellow);
            }
            else
            {
                var owners = _ownerRepository.GetAll();
            foreach (var owner in owners)
            {
                ConsoleHelper.WriteWithColor($"Id: {owner.Id}, Fullname: {owner.Name} {owner.Surname}", ConsoleColor.Magenta);
            }

            OwnerIdInput: ConsoleHelper.WriteWithColor("Enter owner's id: ", ConsoleColor.Blue);

            int id;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto OwnerIdInput;
            }

            var dbOwner = _ownerRepository.Get(id);
                if (dbOwner is null)
                {
                    ConsoleHelper.WriteWithColor("There is no owner with this id", ConsoleColor.Yellow);
                }

                else
                {
                    foreach (var drugstore in dbOwner.Drugstores)
                    {
                        ConsoleHelper.WriteWithColor($"Id: {drugstore.Id}, Name: {drugstore.Name}", ConsoleColor.Magenta);
                    }
                }
            }
        }
        public void Create(Admin admin)
        {
            if (_ownerRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("An owner must be created beforehand", ConsoleColor.Yellow);
            }
            else
            {
                DrugstoreNameInput: ConsoleHelper.WriteWithColor("Enter name: ", ConsoleColor.Blue);
                string name = Console.ReadLine();

                ConsoleHelper.WriteWithColor("Enter address: ", ConsoleColor.Blue);
                string address = Console.ReadLine();

                DrugstoreEmailInput: ConsoleHelper.WriteWithColor("Enter drugstore's email: ", ConsoleColor.Blue);
                string email = Console.ReadLine();

                if (!email.IsEmail())
                {
                    ConsoleHelper.WriteWithColor("Invalid Email format", ConsoleColor.Red);
                    goto DrugstoreEmailInput;
                }

                if (_drugstoreRepository.IsDublicatedEmail(email))
                {
                    ConsoleHelper.WriteWithColor("This email is currently used", ConsoleColor.Red);
                    goto DrugstoreEmailInput;
                }

                DrugstoreContactNumberInput: ConsoleHelper.WriteWithColor("Enter drugstore's contact number: ", ConsoleColor.Blue);
                string contactNumber = Console.ReadLine();

                if (!contactNumber.IsContactNumber())
                {
                    ConsoleHelper.WriteWithColor("Invalid format", ConsoleColor.Red);
                    goto DrugstoreContactNumberInput;
                }

                if (_drugstoreRepository.IsDublicatedContactNumber(contactNumber))
                {
                    ConsoleHelper.WriteWithColor("This contact number is currently used", ConsoleColor.Red);
                    goto DrugstoreContactNumberInput;
                }

                var owners = _ownerRepository.GetAll();
                foreach (var owner in owners)
                {
                    ConsoleHelper.WriteWithColor($"Id: {owner.Id}, Fullname: {owner.Name} {owner.Surname}", ConsoleColor.Magenta);
                }

                OwnerIdInput: ConsoleHelper.WriteWithColor($"Enter owner's id: ", ConsoleColor.Blue);
                int ownerId;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out ownerId);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto OwnerIdInput;
                }

                var dbOwner = _ownerRepository.Get(ownerId);
                if (dbOwner is null)
                {
                    ConsoleHelper.WriteWithColor("No owner with this id!", ConsoleColor.Red);
                    goto OwnerIdInput;
                }
                foreach (var d in dbOwner.Drugstores)
                {
                    if (d.Name == name)
                    {
                        ConsoleHelper.WriteWithColor("Drugstore with this name currently belongs to the chosen owner. Try again.", ConsoleColor.DarkYellow);
                        goto DrugstoreNameInput;
                    }
                }

                var drugstore = new Drugstore
                {
                    Name = name,
                    Address = address,
                    Email = email,
                    ContactNumber = contactNumber,
                    CreatedBy = admin.Username,
                    Owner = dbOwner
                };

                dbOwner.Drugstores.Add(drugstore);
                _drugstoreRepository.Add(drugstore);
                ConsoleHelper.WriteWithColor($"Drugstore was successfully created with Name: {drugstore.Name}\n Address: {drugstore.Address}\n Email: {drugstore.Email}\n Contact Number: {drugstore.ContactNumber}\n Id: {drugstore.Id} \n by: {drugstore.CreatedBy}", ConsoleColor.Green);
            }
        }
        public void Update(Admin admin)
        {
            if (_drugstoreRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No drugstores yet", ConsoleColor.Yellow);
            }
            else
            {
                GetAll();

            DrugstoreIdInput: ConsoleHelper.WriteWithColor("Enter drugstore's id ", ConsoleColor.Blue);

                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto DrugstoreIdInput;
                }


                var drugstore = _drugstoreRepository.Get(id);
                if (drugstore is null)
                {
                    ConsoleHelper.WriteWithColor("No such a drugstore with this Id", ConsoleColor.Red);
                    return;
                }
                InternalUpdate(drugstore, admin);
                ConsoleHelper.WriteWithColor("Updated!", ConsoleColor.Green);
            }
        }
        private void InternalUpdate(Drugstore drugstore, Admin admin)
        {
            ConsoleHelper.WriteWithColor("Enter a new name:", ConsoleColor.Blue);
            string name = Console.ReadLine();

            ConsoleHelper.WriteWithColor("Enter address: ", ConsoleColor.Blue);
            string address = Console.ReadLine();



            drugstore.Name = name;
            drugstore.Address = address;

            drugstore.ModifiedBy = admin.Username;
            _drugstoreRepository.Update(drugstore);
        }
        public void Delete()
        {
            if (_ownerRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No drugstores yet", ConsoleColor.Yellow);
            }
            else
            {
                GetAll();
            DrugstoreIdInput: ConsoleHelper.WriteWithColor("Enter Id: ", ConsoleColor.DarkCyan);

            int id;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Id's format is not correct!", ConsoleColor.Red);
                goto DrugstoreIdInput;
            }

            var dbDrugstore = _drugstoreRepository.Get(id);
                if (dbDrugstore is null)
                    ConsoleHelper.WriteWithColor("There is no such a drugstore with written id", ConsoleColor.Red);
                else
                {
                    foreach (var drug in dbDrugstore.Drugs)
                    {
                        drug.Drugstore = null;
                        _drugRepository.Update(drug);
                    }
                    foreach (var druggist in dbDrugstore.Druggists)
                    {
                        druggist.Drugstore = null;
                        _druggistRepository.Update(druggist);
                    }
                    _drugstoreRepository.Delete(dbDrugstore);
                    ConsoleHelper.WriteWithColor("The drugstore was successfully deleted", ConsoleColor.Green);
                }
            }
        }
        public void Sale()
        {
            if (_drugRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("A drug must be created beforehand", ConsoleColor.Yellow);
            }
            else
            {

                var drugs = _drugRepository.GetAll();
                foreach (var d in drugs)
                {
                    ConsoleHelper.WriteWithColor($"Id: {d.Id} Name: {d.Name} Count: {d.Count} Price: {d.Price} Drugstore: {d.Drugstore.Name} Created by: {d.CreatedBy}", ConsoleColor.Magenta);
                }
                DrugIdInput: ConsoleHelper.WriteWithColor("Enter drug's id ", ConsoleColor.Blue);

                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto DrugIdInput;
                }


                var drug = _drugRepository.Get(id);
                if (drug is null)
                {
                    ConsoleHelper.WriteWithColor("No such a drug with this Id", ConsoleColor.Red);
                }

                DrugsNumberInput: ConsoleHelper.WriteWithColor($"How many {drug.Name} you wanna purchase? ", ConsoleColor.Blue);
                int count;
                isSucceeded = int.TryParse(Console.ReadLine(), out count);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto DrugsNumberInput;
                }

                if (count > drug.Count)
                {
                    ConsoleHelper.WriteWithColor("Not enough drugs are available.", ConsoleColor.Red);
                    goto DrugsNumberInput;
                }

                else
                {
                    drug.Count -= count;
                    ConsoleHelper.WriteWithColor($"""Success! {count} "{drug.Name}"s were purchased, total price: {drug.Price * count}, {drug.Count} left.""", ConsoleColor.Green);
                }

            }
        }
    }
}
