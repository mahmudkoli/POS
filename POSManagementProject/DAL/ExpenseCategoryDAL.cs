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
    public class ExpenseCategoryDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetExpenseCategorySelectList()
        {
            var list = dbContext.ExpenseCategories.Where(x => x.ParentId == null).ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public List<ExpenseCategory> GetExpenseCategoryList()
        {
            var list = dbContext.ExpenseCategories.ToList();
            return list;
        }
        public string GetExpenseCategoryCode()
        {
            long code;

            if (dbContext.ExpenseCategories.ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.ExpenseCategories.Select(x => x.Code).Max());

                if (code >= 999)
                {
                    var allNumbers = Enumerable.Range(1, 999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.ExpenseCategories.Select(x => x.Code))
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
        public bool IsExpenseCategorySaved(ExpenseCategoryVM itemVm)
        {
            ExpenseCategory item = new ExpenseCategory()
            {
                Name = itemVm.Name,
                Code = itemVm.Code,
                Description = itemVm.Description,
                Date = itemVm.Date,
                ParentId = itemVm.ParentId
            };

            dbContext.ExpenseCategories.Add(item);
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



        public ExpenseCategoryVM FindExpenseCategory(long? id)
        {
            var item = dbContext.ExpenseCategories.Find(id);
            var itemParent = dbContext.ExpenseCategories.Find(item.ParentId);
            ExpenseCategoryVM itemVm = null;

            if (item != null)
            {
                itemVm = new ExpenseCategoryVM()
                {
                    Name = item.Name,
                    Code = item.Code,
                    Description = item.Description,
                    Date = item.Date,
                    ParentId = item.ParentId,
                    Parent = itemParent
                };
            }

            return itemVm;
        }

        public bool IsExpenseCategoryUpdated(ExpenseCategoryVM itemVm)
        {
            ExpenseCategory item = new ExpenseCategory()
            {
                Id = itemVm.Id,
                Name = itemVm.Name,
                Code = itemVm.Code,
                Description = itemVm.Description,
                Date = itemVm.Date,
                ParentId = itemVm.ParentId
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsExpenseCategoryDeleted(long id)
        {
            ExpenseCategory item = dbContext.ExpenseCategories.Find(id);
            if (item != null)
            {
                dbContext.ExpenseCategories.Remove(item);
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
            return !dbContext.ExpenseCategories.Any(x => x.Name == name);
        }

        public bool IsUniqueCode(string code, string initialCode)
        {
            if (initialCode == code)
            {
                return true;
            }
            return !dbContext.ExpenseCategories.Any(x => x.Code == code);
        }
    }
}