using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.DAL
{
    public class PartyDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public List<Party> GetPartyList()
        {
            var list = dbContext.Parties.ToList();
            return list;
        }

        public string GetPartyPreCode()
        {
            string year = DateTime.Now.Year.ToString();
            string month = string.Format("{0:00}",DateTime.Now.Month);
            return year+month;
        }

        public string GetPartyCode()
        {
            long code;
            string preCode = GetPartyPreCode();

            if (dbContext.Parties.Where(x => x.PreCode == preCode).ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.Parties.Where(x => x.PreCode == preCode).Select(x => x.Code).Max());

                if (code >= 9999)
                {
                    var allNumbers = Enumerable.Range(1, 9999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.Parties.Where(x => x.PreCode == preCode).Select(x => x.Code))
                        .Select(x => Convert.ToInt64(x));
                    var missingNumbersFirst = allNumbers.Except(currentList).First();
                    code = missingNumbersFirst - 1;
                }
            }
            else
            {
                code = 0;
            }

            return string.Format("{0:0000}", (code + 1));
        }

        public bool IsPartySaved(PartyVM itemVm)
        {
            Party item = new Party()
            {
                Name = itemVm.Name,
                PreCode = itemVm.PreCode,
                Code = itemVm.Code,
                Contact = itemVm.Contact,
                Address = itemVm.Address,
                Email = itemVm.Email,
                Image = itemVm.Image,
                Date = itemVm.Date,
                IsCustomer = itemVm.IsCustomer,
                IsSupplier = itemVm.IsSupplier
            };

            dbContext.Parties.Add(item);
            var isAdded = dbContext.SaveChanges();

            if (isAdded > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            return false;
        }


        public PartyVM FindParty(long? id)
        {
            var item = dbContext.Parties.Find(id);
            PartyVM itemVm = null;

            if (item != null)
            {
                itemVm = new PartyVM()
                {
                    Name = item.Name,
                    PreCode = item.PreCode,
                    Code = item.Code,
                    Contact = item.Contact,
                    Address = item.Address,
                    Email = item.Email,
                    Image = item.Image,
                    Date = item.Date,
                    IsCustomer = item.IsCustomer,
                    IsSupplier = item.IsSupplier,

                };
            }

            return itemVm;
        }

        public bool IsPartyUpdated(PartyVM itemVm)
        {
            Party item = new Party()
            {
                Id = itemVm.Id,
                Name = itemVm.Name,
                PreCode = itemVm.PreCode,
                Code = itemVm.Code,
                Contact = itemVm.Contact,
                Address = itemVm.Address,
                Email = itemVm.Email,
                Image = itemVm.Image,
                Date = itemVm.Date,
                IsCustomer = itemVm.IsCustomer,
                IsSupplier = itemVm.IsSupplier
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsPartyDeleted(long id)
        {
            Party item = dbContext.Parties.Find(id);
            if (item != null)
            {
                dbContext.Parties.Remove(item);
                var isDeleted = dbContext.SaveChanges() > 0;
                return isDeleted;
            }

            return false;
        }

        public bool IsUniqueName(string name, string initialName)
        {
            if (initialName == name)
            {
                return true;
            }
            return !dbContext.Parties.Any(x => x.Name == name);
        }

        public bool IsUniqueCode(string preCode, string code, string initialPreCode, string initialCode)
        {
            if ((initialPreCode + initialCode) == (preCode + code))
            {
                return true;
            }
            return !dbContext.Parties.Any(x => x.Code == code && x.PreCode == preCode);
        }

    }
}