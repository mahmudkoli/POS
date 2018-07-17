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
    public class ExpenseItemDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetExpenseItemSelectList()
        {
            var list = dbContext.ExpenseCategories.Select(x => x).Distinct().ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public List<ExpenseItem> GetExpenseItemList()
        {
            var list = dbContext.ExpenseItems.ToList();
            return list;
        }
        public bool IsExpenseItemSaved(ExpenseItemVM itemVm)
        {
            ExpenseItem item = new ExpenseItem()
            {
                Name = itemVm.Name,
                PreCode = itemVm.PreCode,
                Code = itemVm.Code,
                Description = itemVm.Description,
                Date = itemVm.Date,
                CategoryId = itemVm.CategoryId
            };

            dbContext.ExpenseItems.Add(item);
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


        public string GetExpenseCategoryCode(long id)
        {
            var code = dbContext.ExpenseCategories.Where(x => x.Id == id).Select(x => x.Code).FirstOrDefault();
            return code;
        }

        public string GetExpenseItemCode(long id)
        {
            long code;
            string preCode = GetExpenseCategoryCode(id);

            if (dbContext.ExpenseItems.Where(x => x.PreCode == preCode).ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.ExpenseItems.Where(x => x.PreCode == preCode).Select(x => x.Code).Max());

                if (code >= 999999)
                {
                    var allNumbers = Enumerable.Range(1, 999999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.ExpenseItems.Where(x => x.PreCode == preCode).Select(x => x.Code))
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

        public ExpenseItemVM FindExpenseItem(long? id)
        {
            var item = dbContext.ExpenseItems.Find(id);
            var itemCategory = dbContext.ExpenseCategories.Find(item.CategoryId);
            ExpenseItemVM itemVm = null;

            if (item != null)
            {
                itemVm = new ExpenseItemVM()
                {
                    Name = item.Name,
                    PreCode = item.PreCode,
                    Code = item.Code,
                    Description = item.Description,
                    Date = item.Date,
                    CategoryId = item.CategoryId,
                    Category = itemCategory
                };
            }

            return itemVm;
        }

        public bool IsExpenseItemUpdated(ExpenseItemVM itemVm)
        {
            ExpenseItem item = new ExpenseItem()
            {
                Id = itemVm.Id,
                Name = itemVm.Name,
                PreCode = itemVm.PreCode,
                Code = itemVm.Code,
                Description = itemVm.Description,
                Date = itemVm.Date,
                CategoryId = itemVm.CategoryId
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsExpenseItemDeleted(long id)
        {
            ExpenseItem item = dbContext.ExpenseItems.Find(id);
            if (item != null)
            {
                dbContext.ExpenseItems.Remove(item);
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
            return !dbContext.ExpenseItems.Any(x => x.Name == name);
        }

        public bool IsUniqueCode(string preCode, string code, string initialPreCode, string initialCode)
        {
            if ((initialPreCode + initialCode) == (preCode + code))
            {
                return true;
            }
            return !dbContext.ExpenseItems.Any(x => x.Code == code && x.PreCode == preCode);
        }


    }
}