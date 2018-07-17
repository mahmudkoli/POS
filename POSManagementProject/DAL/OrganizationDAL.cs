using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.DAL
{
    public class OrganizationDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public List<Organization> GetOrganizationList()
        {
            var list = dbContext.Organizations.ToList();
            return list;
        }
        public string GetOrganizationCode()
        {
            long code;

            if (dbContext.Organizations.ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.Organizations.Select(x => x.Code).Max());

                if (code >= 999)
                {
                    var allNumbers = Enumerable.Range(1, 999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.Organizations.Select(x => x.Code))
                        .Select(x => Convert.ToInt64(x));
                    var missingNumbersFirst = allNumbers.Except(currentList).First();
                    code = missingNumbersFirst - 1;
                }
            }
            else
            {
                code = 0;
            }

            return string.Format("{0:000}", (code + 1));
        }
        public bool IsOrganizationSaved(OrganizationVM itemVm)
        {
            Organization item = new Organization()
            {
                Name = itemVm.Name,
                Code = itemVm.Code,
                Contact = itemVm.Contact,
                Address = itemVm.Address,
                Image = itemVm.Image,
                Date = itemVm.Date
                
            };

            dbContext.Organizations.Add(item);
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



        public OrganizationVM FindOrganization(long? id)
        {
            var item = dbContext.Organizations.Find(id);
            OrganizationVM itemVm = null;

            if (item != null)
            {
                itemVm = new OrganizationVM()
                {
                    Name = item.Name,
                    Code = item.Code,
                    Contact = item.Contact,
                    Address = item.Address,
                    Image = item.Image,
                    Date = item.Date
                };
            }

            return itemVm;
        }

        public bool IsOrganizationUpdated(OrganizationVM itemVm)
        {
            Organization item = new Organization()
            {
                Id = itemVm.Id,
                Name = itemVm.Name,
                Code = itemVm.Code,
                Contact = itemVm.Contact,
                Address = itemVm.Address,
                Image = itemVm.Image,
                Date = itemVm.Date
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsOrganizationDeleted(long id)
        {
            Organization item = dbContext.Organizations.Find(id);
            if (item != null)
            {
                dbContext.Organizations.Remove(item);
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
            return !dbContext.Organizations.Any(x => x.Name == name);
        }

        public bool IsUniqueCode(string code, string initialCode)
        {
            if (initialCode == code)
            {
                return true;
            }
            return !dbContext.Organizations.Any(x => x.Code == code);
        }
    }
}