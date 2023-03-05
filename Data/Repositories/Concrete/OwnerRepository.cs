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
    public class OwnerRepository : IOwnerRepository
    {
        static int id;
        public List<Owner> GetAll()
        {
            return DbContext.Owners;
        }

        public Owner Get(int id)
        {
            return DbContext.Owners.FirstOrDefault(o => o.Id == id);
        }

        public void Add(Owner owner)
        {
            id++;
            owner.Id = id;
            owner.CreatedAt = DateTime.Now;
            DbContext.Owners.Add(owner);
        }

        public void Update(Owner owner)
        {
            var dbOwner = DbContext.Owners.FirstOrDefault(o => o.Id == owner.Id);
            if (dbOwner is not null)
            {
                dbOwner.Name = dbOwner.Name;
                dbOwner.Surname = dbOwner.Surname;
                dbOwner.ModifiedAt = DateTime.Now;
            }
        }

        public void Delete(Owner owner)
        {
            DbContext.Owners.Remove(owner);
        }
    }
}
