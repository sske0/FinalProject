using Core.Entities;
using Core.Extensions;
using Core.Helpers;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class DrugService
    {
        private readonly DrugstoreRepository _drugstoreRepository;
        private readonly DrugRepository _drugRepository;
        private readonly DrugstoreService _drugstoreService;


        public DrugService()
        {
            _drugstoreRepository = new DrugstoreRepository();
            _drugRepository = new DrugRepository();
            _drugstoreService = new DrugstoreService();
        }

        public void GetAll()
        {
            var drugs = _drugRepository.GetAll();
            foreach (var drug in drugs)
            {
                ConsoleHelper.WriteWithColor($"Id: {drug.Id} Name: {drug.Name} Count: {drug.Count} Price: {drug.Price} Drugstore: {drug.Drugstore.Name} Created by: {drug.CreatedBy}", ConsoleColor.Magenta);
            }
        }
        public void GetAllDrugsByDrugstore()
        {
            if (_drugRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("A drug must be created beforehand", ConsoleColor.Yellow);
            }
            else
            {
                _drugstoreService.GetAll();

            DrugstoreIdInput: ConsoleHelper.WriteWithColor("Enter drugstore's id: ", ConsoleColor.Blue);
            int drugstoreId;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out drugstoreId);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto DrugstoreIdInput;
            }

            var drugstore = _drugstoreRepository.Get(drugstoreId);
            if (drugstore is null)
            {
                ConsoleHelper.WriteWithColor("There is no drugstore with this id!", ConsoleColor.Yellow);
                return;
            }

                if (drugstore.Drugs.Count == 0)
                {
                    ConsoleHelper.WriteWithColor("No drugs in this drugstore", ConsoleColor.Yellow);
                    return;
                }
                else
                {
                    foreach (var drug in drugstore.Drugs)
                    {
                        ConsoleHelper.WriteWithColor($"Id: {drug.Id} Name: {drug.Name} Count: {drug.Count} Price: {drug.Price} Drugstore: {drug.Drugstore.Name} Created by: {drug.CreatedBy}", ConsoleColor.Magenta);
                    }
                }
            }
        }
        public void Create(Admin admin)
        {
            if (_drugstoreRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("A drugstore must be created beforehand", ConsoleColor.Yellow);
            }
            else
            {
                DrugNameInput: ConsoleHelper.WriteWithColor("Enter name: ", ConsoleColor.Blue);
                string name = Console.ReadLine();
               
                var drugstores = _drugstoreRepository.GetAll();
                foreach (var drugstore in drugstores)
                {
                    ConsoleHelper.WriteWithColor($"Id: {drugstore.Id}, Name: {drugstore.Name} Owner: {drugstore.Owner.Name}", ConsoleColor.Magenta);
                }

                DrugstoreIdInput: ConsoleHelper.WriteWithColor("Enter drugstore's id: ", ConsoleColor.Blue);
                int drugstoreId;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out drugstoreId);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto DrugstoreIdInput;
                } 

                var dbDrugstore = _drugstoreRepository.Get(drugstoreId);
                if (dbDrugstore is null)
                {
                    ConsoleHelper.WriteWithColor("No drugstore with this id!", ConsoleColor.Red);
                    goto DrugstoreIdInput;
                }

                foreach (var d in dbDrugstore.Drugs)
                {
                    if (d.Name == name)
                    {
                        ConsoleHelper.WriteWithColor("This drug has been already added to chosen drugstore. Try again.", ConsoleColor.DarkYellow);
                        goto DrugNameInput;
                    }
                }

                DrugPriceInput: ConsoleHelper.WriteWithColor("Enter the price: ", ConsoleColor.Blue);
                decimal price;
                isSucceeded = decimal.TryParse(Console.ReadLine(), out price);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto DrugPriceInput;
                }

                if (price <= 0)
                {
                    ConsoleHelper.WriteWithColor("Negative or zero prices cannot be entered!", ConsoleColor.Red);
                    goto DrugPriceInput;
                }



                DrugCountInput: ConsoleHelper.WriteWithColor("Enter the number of drugs: ", ConsoleColor.Blue);
                int count;
                isSucceeded = int.TryParse(Console.ReadLine(), out count);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto DrugCountInput;
                }

                if (count <= 0)
                {
                    ConsoleHelper.WriteWithColor("Negative numbers or zero cannot be entered!", ConsoleColor.Red);
                    goto DrugCountInput;
                }

                var drug = new Drug
                {
                    Name = name,
                    Drugstore = dbDrugstore,
                    Price = price,
                    Count = count,
                    CreatedBy = admin.Username
                };

                dbDrugstore.Drugs.Add(drug);
                _drugRepository.Add(drug);
                ConsoleHelper.WriteWithColor($"Drug was successfully created with Name: {drug.Name}\n Drugstore: {dbDrugstore.Name} Drug's Id: {drug.Id}\n Price: {drug.Price}\n Count: {drug.Count}\n By: {drug.CreatedBy}", ConsoleColor.Green);
            }
        }
        public void Update(Admin admin)
        {
            if (_drugRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No drugs yet", ConsoleColor.Yellow);
            }
            else
            {
                GetAll();

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
                InternalUpdate(drug, admin);
                ConsoleHelper.WriteWithColor("Updated!", ConsoleColor.Green);
            }
        }
        public void InternalUpdate(Drug drug, Admin admin)
        {
            DrugNameInput: ConsoleHelper.WriteWithColor("Enter a new name:", ConsoleColor.Blue);
            string name = Console.ReadLine();

           
            DrugPriceInput: ConsoleHelper.WriteWithColor("Enter a new price: ", ConsoleColor.Blue);
            decimal price;
            bool isSucceeded = decimal.TryParse(Console.ReadLine(), out price);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto DrugPriceInput;
            }

            if (price <= 0)
            {
                ConsoleHelper.WriteWithColor("Negative or zero prices cannot be entered!", ConsoleColor.Red);
                goto DrugPriceInput;
            }



            DrugCountInput: ConsoleHelper.WriteWithColor("Enter the number of drugs: ", ConsoleColor.Blue);
            int count;
            isSucceeded = int.TryParse(Console.ReadLine(), out count);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto DrugCountInput;
            }

            if (count <= 0)
            {
                ConsoleHelper.WriteWithColor("Negative numbers or zero cannot be entered!", ConsoleColor.Red);
                goto DrugCountInput;
            }



            drug.Name = name;
            drug.Count = count;
            drug.Price = price;


            drug.ModifiedBy = admin.Username;
            _drugRepository.Update(drug);
        }
        public void Detele()
        {
            if (_drugRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No drugs yet", ConsoleColor.Yellow);
            }
            else
            {
                GetAll();
            DrugIdInput: ConsoleHelper.WriteWithColor("Enter Id: ", ConsoleColor.Blue);

                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Id's format is incorrect!", ConsoleColor.Red);
                    goto DrugIdInput;
                }

                var drug = _drugRepository.Get(id);
                if (drug is null)
                {
                    ConsoleHelper.WriteWithColor("No drug with this id", ConsoleColor.Yellow);
                    goto DrugIdInput;
                }

                _drugRepository.Delete(drug);
                ConsoleHelper.WriteWithColor($"{drug.Name} was deleted", ConsoleColor.Green);
            }
        }
        public void Filter()
        {
            if (_drugRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("A drug must be created beforehand", ConsoleColor.Yellow);
            }
            else
            {
            DrugPriceInput: ConsoleHelper.WriteWithColor("Enter the price: ", ConsoleColor.Blue);
            decimal price;
            bool isSucceeded = decimal.TryParse(Console.ReadLine(), out price);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto DrugPriceInput;
            }

            if (price <= 0)
            {
                ConsoleHelper.WriteWithColor("Negative or zero prices cannot be entered!", ConsoleColor.Red);
                goto DrugPriceInput;
            }
            var drugs = _drugRepository.GetAll();

                foreach (var drug in drugs)
                {
                    if (drug.Price < price)
                    {
                        ConsoleHelper.WriteWithColor($"Id: {drug.Id} Name: {drug.Name} Count: {drug.Count} Price: {drug.Price} Drugstore: {drug.Drugstore.Name} Created by: {drug.CreatedBy}", ConsoleColor.Magenta);
                    }
                }
            }
            
        }

    }

}
