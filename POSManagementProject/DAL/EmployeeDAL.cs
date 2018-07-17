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
    public class EmployeeDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetEmployeeBranchSelectList()
        {
            var list = dbContext.Branches.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Address, Value = li.Id.ToString() }));

            return selectList;
        }
        public IEnumerable<SelectListItem> GetEmployeeSelectList()
        {
            var list = dbContext.Employees.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public List<Employee> GetEmployeeList()
        {
            var list = dbContext.Employees.ToList();
            return list;
        }
        public string GetEmployeeCode()
        {
            long code;

            if (dbContext.Employees.ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.Employees.Select(x => x.Code).Max());

                if (code >= 999999)
                {
                    var allNumbers = Enumerable.Range(1, 999999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.Employees.Select(x => x.Code))
                        .Select(x => Convert.ToInt64(x));
                    var missingNumbersFirst = allNumbers.Except(currentList).First();
                    code = missingNumbersFirst - 1;
                }
            }
            else
            {
                code = 0;
            }

            return string.Format("{0:000000}", (code + 1));
        }
        public bool IsEmployeeSaved(EmployeeVM itemVm)
        {
            Employee item = new Employee()
            {
                Name = itemVm.Name,
                Code = itemVm.Code,
                BranchId = itemVm.BranchId,
                JoiningDate = itemVm.JoiningDate,
                Image = itemVm.Image,
                Contact = itemVm.Contact,
                Email = itemVm.Email,
                ReferenceId = itemVm.ReferenceId,
                EmergencyContact = itemVm.EmergencyContact,
                NID = itemVm.NID,
                FathersName = itemVm.FathersName,
                MothersName = itemVm.MothersName,
                PresentAddress = itemVm.PresentAddress,
                PermanentAddress = itemVm.PermanentAddress,
                Date = itemVm.Date
            };

            dbContext.Employees.Add(item);
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


        public EmployeeVM FindEmployee(long? id)
        {
            var item = dbContext.Employees.Where(x => x.Id == id)
                .Include(x => x.Branch).Include(x => x.Reference).FirstOrDefault();
            EmployeeVM itemVm = null;

            if (item != null)
            {
                itemVm = new EmployeeVM()
                {
                    Name = item.Name,
                    Code = item.Code,
                    BranchId = item.BranchId,
                    JoiningDate = item.JoiningDate,
                    Image = item.Image,
                    Contact = item.Contact,
                    Email = item.Email,
                    ReferenceId = item.ReferenceId,
                    EmergencyContact = item.EmergencyContact,
                    NID = item.NID,
                    FathersName = item.FathersName,
                    MothersName = item.MothersName,
                    PresentAddress = item.PresentAddress,
                    PermanentAddress = item.PermanentAddress,
                    Date = item.Date,
                    Branch = item.Branch,
                    Reference = item.Reference
                };
            }

            return itemVm;
        }

        public bool IsEmployeeUpdated(EmployeeVM itemVm)
        {
            Employee item = new Employee()
            {
                Id = itemVm.Id,
                Name = itemVm.Name,
                Code = itemVm.Code,
                BranchId = itemVm.BranchId,
                JoiningDate = itemVm.JoiningDate,
                Image = itemVm.Image,
                Contact = itemVm.Contact,
                Email = itemVm.Email,
                ReferenceId = itemVm.ReferenceId,
                EmergencyContact = itemVm.EmergencyContact,
                NID = itemVm.NID,
                FathersName = itemVm.FathersName,
                MothersName = itemVm.MothersName,
                PresentAddress = itemVm.PresentAddress,
                PermanentAddress = itemVm.PermanentAddress,
                Date = itemVm.Date
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsEmployeeDeleted(long id)
        {
            Employee item = dbContext.Employees.Find(id);
            if (item != null)
            {
                dbContext.Employees.Remove(item);
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
            return !dbContext.Employees.Any(x => x.Name == name);
        }

        public bool IsUniqueCode(string code, string initialCode)
        {
            if (initialCode == code)
            {
                return true;
            }
            return !dbContext.Employees.Any(x => x.Code == code);
        }
    }
}