using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete
{
    public class DrugstoreRepository : IDrugstoreRepository
    {
        static int id;
        public List<Drugstore> GetAll()
        {
            return DbContext.Drugstores;
        }

        public Drugstore Get(int id)
        {
            return DbContext.Drugstores.FirstOrDefault(d => d.Id == id);
        }

        public void Add(Drugstore drugstore)
        {
            id++;
            drugstore.Id = id;
            drugstore.CreatedAt = DateTime.Now;
            DbContext.Drugstores.Add(drugstore);
        }

        public void Update(Drugstore drugstore)
        {
            var dbDrugstore = DbContext.Drugstores.FirstOrDefault(d => d.Id == drugstore.Id);
            if (dbDrugstore is not null)
            {
                dbDrugstore.Name = dbDrugstore.Name;
                dbDrugstore.Address = dbDrugstore.Address;
                dbDrugstore.ContactNumber = dbDrugstore.ContactNumber;
                dbDrugstore.Email = dbDrugstore.Email;
                dbDrugstore.ModifiedAt = DateTime.Now;
            }
        }

        public void Delete(Drugstore drugstore)
        {
            DbContext.Drugstores.Remove(drugstore);
        }

        public bool IsDublicatedEmail(string email)
        {
            return DbContext.Drugstores.Any(d => d.Email == email);
        }

        public bool IsDublicatedContactNumber(string contactNumber)
        {
            return DbContext.Drugstores.Any(d => d.ContactNumber == contactNumber);
        }
    }
}
