using Core.Entities;
using Core.Helpers;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class DruggistService
    {
        private readonly DrugstoreRepository _drugstoreRepository;
        private readonly OwnerRepository _ownerRepository;
        private readonly DrugRepository _drugRepository;
        private readonly DruggistRepository _druggistRepository;

        private readonly DrugstoreService _drugstoreService;


        public DruggistService()
        {
            _drugstoreRepository = new DrugstoreRepository();
            _ownerRepository = new OwnerRepository();
            _drugRepository = new DrugRepository();
            _druggistRepository = new DruggistRepository();

            _drugstoreService = new DrugstoreService();
        }
        public void GetAll()
        {
            if (_druggistRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No druggists yet", ConsoleColor.Yellow);
            }
            var druggists = _druggistRepository.GetAll();
            foreach (var druggist in druggists)
            {
                ConsoleHelper.WriteWithColor($"Id: {druggist.Id} Name: {druggist.Name} Surname: {druggist.Surname}, Age: {druggist.Age}, Experience: {druggist.Experience} Created by: {druggist.CreatedBy}", ConsoleColor.Magenta);
            }
        }
        public void GetAllDruggistsByDrugstore()
        {
            if (_druggistRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No druggists yet", ConsoleColor.Yellow);
                return;
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

                if (drugstore.Druggists.Count == 0)
                {
                    ConsoleHelper.WriteWithColor("No druggists in this drugstore", ConsoleColor.Yellow);
                    return;
                }
                else
                {
                    foreach (var druggist in drugstore.Druggists)
                    {
                        ConsoleHelper.WriteWithColor($"Id: {druggist.Id} Name: {druggist.Name} Surname: {druggist.Surname} Age: {druggist.Age} Experience: {druggist.Experience} Drugstore: {druggist.Drugstore.Name} Created by: {druggist.CreatedBy}", ConsoleColor.Magenta);
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
                ConsoleHelper.WriteWithColor("Enter name: ", ConsoleColor.Blue);
                string name = Console.ReadLine();


                ConsoleHelper.WriteWithColor("Enter surname: ", ConsoleColor.Blue);
                string surname = Console.ReadLine();

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

                

                AgeInput: ConsoleHelper.WriteWithColor("Enter age: ", ConsoleColor.Blue);
                byte age;
                isSucceeded = byte.TryParse(Console.ReadLine(), out age);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto AgeInput;
                }

                if (age <= 0 || age > 127)
                {
                    ConsoleHelper.WriteWithColor("Enter the real age!", ConsoleColor.Red);
                    goto AgeInput;
                }

                if (age < 18)
                {
                    ConsoleHelper.WriteWithColor("Minimum age is 18", ConsoleColor.Yellow);
                    goto AgeInput;
                }

                ExperienceInput: ConsoleHelper.WriteWithColor("Enter your experience in years (Note that experience in age of younger than 18 does not count and experience cannot be more than age -_-): ", ConsoleColor.Blue);
                byte experience;
                isSucceeded = byte.TryParse(Console.ReadLine(), out experience);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto ExperienceInput;
                }

                if (18 > age - experience)
                {
                    ConsoleHelper.WriteWithColor("Invalid experience in terms of requirements. Try again.", ConsoleColor.Yellow);
                    goto ExperienceInput;
                }

                var druggist = new Druggist
                {
                    Name = name,
                    Surname = surname,
                    Age = age,
                    Experience= experience,
                    Drugstore = dbDrugstore,
                    CreatedBy = admin.Username
                };

                dbDrugstore.Druggists.Add(druggist);
                _druggistRepository.Add(druggist);
                ConsoleHelper.WriteWithColor($"Druggist *{druggist.Name} {druggist.Surname}* was successfully created with\n Id: {druggist.Id}\n Drugstore: {dbDrugstore.Name}\n Age: {druggist.Age}\n Experience: {druggist.Experience}\n By: {druggist.CreatedBy}", ConsoleColor.Green);
            }
        }
        public void Update(Admin admin)
        {
            if (_druggistRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No druggists yet", ConsoleColor.Yellow);
                return;
            }
            else
            {
                GetAll();
                DruggistIdInput: ConsoleHelper.WriteWithColor("Enter drggist's id: ", ConsoleColor.Blue);
                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto DruggistIdInput;
                }

                var druggist = _druggistRepository.Get(id);
                if (druggist is null)
                {
                    ConsoleHelper.WriteWithColor("There is no druggist with this id", ConsoleColor.Yellow);
                    goto DruggistIdInput;
                }
                ConsoleHelper.WriteWithColor("Enter a new name: ", ConsoleColor.Blue);
                string name = Console.ReadLine();

                ConsoleHelper.WriteWithColor("Enter a new surname: ", ConsoleColor.Blue);
                string surname = Console.ReadLine();

                AgeInput: ConsoleHelper.WriteWithColor("New age: ", ConsoleColor.Blue);
                byte age;
                isSucceeded = byte.TryParse(Console.ReadLine(), out age);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto AgeInput;
                }

                if (age <= 0 || age > 127)
                {
                    ConsoleHelper.WriteWithColor("Enter the real age!", ConsoleColor.Red);
                    goto AgeInput;
                }

                if (age < 18)
                {
                    ConsoleHelper.WriteWithColor("Minimum age is 18", ConsoleColor.Yellow);
                    goto AgeInput;
                }

                ExperienceInput: ConsoleHelper.WriteWithColor("Enter your experience in years (Note that experience in age of younger than 18 does not count and experience cannot be more than age -_-): ", ConsoleColor.Blue);
                byte experience;
                isSucceeded = byte.TryParse(Console.ReadLine(), out experience);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto ExperienceInput;
                }

                if (18 > age - experience)
                {
                    ConsoleHelper.WriteWithColor("Invalid experience in terms of requirements. Try again.", ConsoleColor.Yellow);
                    goto ExperienceInput;
                }

                druggist.Name = name;
                druggist.Surname = surname;
                druggist.Age = age;
                druggist.Experience = experience;

                _druggistRepository.Update(druggist);

                ConsoleHelper.WriteWithColor($"{druggist.Name} {druggist.Surname} was updated\n Age: {druggist.Age},experience: {druggist.Experience}", ConsoleColor.Green);
            }
        }
        public void Delete()
        {
            if (_druggistRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No druggists yet", ConsoleColor.Yellow);
            }
            else
            {
                GetAll();
                DruggistIdInput: ConsoleHelper.WriteWithColor("Enter Id: ", ConsoleColor.Blue);

                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Id's format is incorrect!", ConsoleColor.Red);
                    goto DruggistIdInput;
                }

                var druggist = _druggistRepository.Get(id);
                if (druggist is null)
                {
                    ConsoleHelper.WriteWithColor("No druggist with this id", ConsoleColor.Yellow);
                    goto DruggistIdInput;
                }

                _druggistRepository.Delete(druggist);
                ConsoleHelper.WriteWithColor($"{druggist.Name} was deleted", ConsoleColor.Green);
            }
        }
    }
}
