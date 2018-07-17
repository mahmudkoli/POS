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
    public class BranchDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetBranchSelectList()
        {
            var list = dbContext.Organizations.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public List<Branch> GetBranchList()
        {
            var list = dbContext.Branches.ToList();
            return list;
        }
        public string GetBranchCode()
        {
            long code;

            if (dbContext.Branches.ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.Branches.Select(x => x.Code).Max());

                if (code >= 999)
                {
                    var allNumbers = Enumerable.Range(1, 999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.Branches.Select(x => x.Code))
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
        public bool IsBranchSaved(BranchVM itemVm)
        {
            Branch item = new Branch()
            {
                Code = itemVm.Code,
                Contact = itemVm.Contact,
                Address = itemVm.Address,
                OrganizationId = itemVm.OrganizationId,
                Date = itemVm.Date
            };

            dbContext.Branches.Add(item);
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



        public BranchVM FindBranch(long? id)
        {
            var item = dbContext.Branches.Find(id);
            var itemOrganization = dbContext.Organizations.Find(item.OrganizationId);
            BranchVM itemVm = null;

            if (item != null)
            {
                itemVm = new BranchVM()
                {
                    Code = item.Code,
                    Contact = item.Contact,
                    Address = item.Address,
                    Date = item.Date,
                    OrganizationId = item.OrganizationId,
                    Organization = itemOrganization
                };
            }

            return itemVm;
        }

        public bool IsBranchUpdated(BranchVM itemVm)
        {
            Branch item = new Branch()
            {
                Id = itemVm.Id,
                Code = itemVm.Code,
                Contact = itemVm.Contact,
                Address = itemVm.Address,
                OrganizationId = itemVm.OrganizationId,
                Date = itemVm.Date
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsBranchDeleted(long id)
        {
            Branch item = dbContext.Branches.Find(id);
            if (item != null)
            {
                dbContext.Branches.Remove(item);
                var isDeleted = dbContext.SaveChanges() > 0;
                return isDeleted;
            }

            return false;
        }

        public bool IsUniqueCode(string code, string initialCode)
        {
            if (initialCode == code)
            {
                return true;
            }
            return !dbContext.Branches.Any(x => x.Code == code);
        }
    }
}