using Core.Constants;
using Core.Helpers;
using Data;
using FinalProject.Services;
using Presentation.Services;
using System.Text;

namespace FinalProject
{
    public class Program
    {
        private readonly static AdminService _adminService;
        private readonly static OwnerService _ownerService;
        private readonly static DrugstoreService _drugstoreService;
        private readonly static DrugService _drugService;
        private readonly static DruggistService _druggistService;




        static Program()
        {
            Console.OutputEncoding = Encoding.UTF8;
            DbInitializer.SeedAdmins();

            _adminService = new AdminService();
            _ownerService = new OwnerService();
            _drugstoreService = new DrugstoreService();
            _drugService = new DrugService();
            _druggistService = new DruggistService();
        }
        static void Main()
        {
        Authorize: var admin = _adminService.Authorize();
            if (admin is not null)
            {
                ConsoleHelper.WriteWithColor($"Welcome,{admin.Username}!", ConsoleColor.Cyan);

                while (true)
                {
                MainMenu: ConsoleHelper.WriteWithColor("1 - Owners", ConsoleColor.DarkCyan);
                    ConsoleHelper.WriteWithColor("2 - Drugstores", ConsoleColor.DarkCyan);
                    ConsoleHelper.WriteWithColor("3 - Druggists", ConsoleColor.DarkCyan);
                    ConsoleHelper.WriteWithColor("4 - Drugs", ConsoleColor.DarkCyan);
                    ConsoleHelper.WriteWithColor("0 - Logout", ConsoleColor.DarkCyan);

                    int number;
                    bool isSucceeded = int.TryParse(Console.ReadLine(), out number);
                    if (!isSucceeded)
                    {
                        ConsoleHelper.WriteWithColor("invalid format!", ConsoleColor.Red);
                        goto MainMenu;
                    }
                    else
                    {
                        switch (number)
                        {
                            case (int)MainMenuOptions.Owners:
                                while (true)
                                {
                                OwnersMenu: ConsoleHelper.WriteWithColor("1 - Create An Owner", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("2 - Update Owner", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("3 - Delete Owner", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("4 - Get All Owners", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("0 - Back to Main Menu", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("--- Select your option ---", ConsoleColor.DarkCyan);
                                    isSucceeded = int.TryParse(Console.ReadLine(), out number);
                                    if (!isSucceeded)
                                    {
                                        ConsoleHelper.WriteWithColor("Inputed number's format is not valid", ConsoleColor.Red);
                                    }
                                    else
                                    {
                                        switch (number)
                                        {
                                            case (int)OwnerOptions.CreateOwner:
                                                _ownerService.Create(admin);
                                                break;
                                            case (int)OwnerOptions.UpdateOwner:
                                                _ownerService.Update();
                                                break;
                                            case (int)OwnerOptions.DeleteOwner:
                                                _ownerService.Delete();
                                                break;
                                            case (int)OwnerOptions.GetAllOwners:
                                                _ownerService.GetAll();
                                                break;
                                            case (int)OwnerOptions.BackToMainMenu:
                                                goto MainMenu;
                                            default:
                                                ConsoleHelper.WriteWithColor("Choose a number from 0 to 4!", ConsoleColor.Red);
                                                goto OwnersMenu;
                                        }
                                    }
                                }
                            case (int)MainMenuOptions.Drugstores:
                                while (true)
                                {
                                DrugstoresMenu: ConsoleHelper.WriteWithColor("1 - Create A Drugstore", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("2 - Update Drugstore", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("3 - Delete Drugstore", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("4 - Get All Drugstores", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("5 - Get All Drugstores By Owner", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("6 - Sale", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("0 - Back to Main Menu", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("--- Select your option ---", ConsoleColor.DarkCyan);
                                    isSucceeded = int.TryParse(Console.ReadLine(), out number);
                                    if (!isSucceeded)
                                    {
                                        ConsoleHelper.WriteWithColor("Inputed number's format is not valid", ConsoleColor.Red);
                                    }
                                    else
                                    {
                                        switch (number)
                                        {
                                            case (int)DrugstoreOptions.CreateDrugstore:
                                                _drugstoreService.Create(admin);
                                                break;
                                            case (int)DrugstoreOptions.UpdateDrugstore:
                                                _drugstoreService.Update(admin);
                                                break;
                                            case (int)DrugstoreOptions.DeleteDrugstore:
                                                _drugstoreService.Delete();
                                                break;
                                            case (int)DrugstoreOptions.GetAllDrugstores:
                                                _drugstoreService.GetAll();
                                                break;
                                            case (int)DrugstoreOptions.GetAllDrugstoresByOwner:
                                                _drugstoreService.GetAllDrugstoresByOwner();
                                                break;
                                            case (int)DrugstoreOptions.Sale:
                                                _drugstoreService.Sale();
                                                break;
                                            case (int)DrugstoreOptions.BackToMainMenu:
                                                goto MainMenu;
                                            default:
                                                ConsoleHelper.WriteWithColor("Choose a number from 0 to 5!", ConsoleColor.Red);
                                                goto DrugstoresMenu;
                                        }
                                    }
                                }
                            case (int)MainMenuOptions.Druggists:
                                while (true)
                                {
                                DruggistsMenu: ConsoleHelper.WriteWithColor("1 - Create A Druggist", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("2 - Update Druggist", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("3 - Delete Druggist", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("4 - Get All Druggists", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("5 - Get All Druggists By Drugstore", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("0 - Back to Main Menu", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("--- Select your option ---", ConsoleColor.DarkCyan);
                                    isSucceeded = int.TryParse(Console.ReadLine(), out number);
                                    if (!isSucceeded)
                                    {
                                        ConsoleHelper.WriteWithColor("Inputed number's format is not valid", ConsoleColor.Red);
                                    }
                                    else
                                    {
                                        switch (number)
                                        {
                                            case (int)DruggistOptions.CreateDruggist:
                                                _druggistService.Create(admin);
                                                break;
                                            case (int)DruggistOptions.UpdateDruggist:
                                                _druggistService.Update(admin);
                                                break;
                                            case (int)DruggistOptions.DeleteDruggist:
                                                _druggistService.Delete();
                                                break;
                                            case (int)DruggistOptions.GetAllDruggists:
                                                _druggistService.GetAll();
                                                break;
                                            case (int)DruggistOptions.GetAllDruggistsByDrugstore:
                                                _druggistService.GetAllDruggistsByDrugstore();
                                                break;
                                            case (int)DruggistOptions.BackToMainMenu:
                                                goto MainMenu;
                                            default:
                                                ConsoleHelper.WriteWithColor("Choose a number from 0 to 5!", ConsoleColor.Red);
                                                goto DruggistsMenu;
                                        }
                                    }
                                }
                            case (int)MainMenuOptions.Drugs:
                                while (true)
                                {
                                DrugsMenu: ConsoleHelper.WriteWithColor("1 - Create A Drug", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("2 - Update Drug", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("3 - Delete Drug", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("4 - Get All Drugs", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("5 - Get All Drugs By Drugstore", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("6 - Filter", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("0 - Back to Main Menu", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("--- Select your option ---", ConsoleColor.DarkCyan);
                                    isSucceeded = int.TryParse(Console.ReadLine(), out number);
                                    if (!isSucceeded)
                                    {
                                        ConsoleHelper.WriteWithColor("Inputed number's format is not valid", ConsoleColor.Red);
                                    }
                                    else
                                    {
                                        switch (number)
                                        {
                                            case (int)DrugOptions.CreateDrug:
                                                _drugService.Create(admin);
                                                break;
                                            case (int)DrugOptions.UpdateDrug:
                                                _drugService.Update(admin);
                                                break;
                                            case (int)DrugOptions.DeleteDrug:
                                                _drugService.Detele();
                                                break;
                                            case (int)DrugOptions.GetAllDrugs:
                                                _drugService.GetAll();
                                                break;
                                            case (int)DrugOptions.GetAllDrugsByDrugstore:
                                                _drugService.GetAllDrugsByDrugstore();
                                                break;
                                            case (int)DrugOptions.Filter:
                                                _drugService.Filter();
                                                break;
                                            case (int)DrugOptions.BackToMainMenu:
                                                goto MainMenu;
                                            default:
                                                ConsoleHelper.WriteWithColor("Choose a number from 0 to 6!", ConsoleColor.Red);
                                                goto DrugsMenu;
                                        }
                                    }
                                }
                            case (int)MainMenuOptions.Logout:
                                goto Authorize;
                            default:
                                ConsoleHelper.WriteWithColor("There is no such an option!", ConsoleColor.Red);
                                goto MainMenu;
                        }
                    }
                }
            }
        }
    }
}