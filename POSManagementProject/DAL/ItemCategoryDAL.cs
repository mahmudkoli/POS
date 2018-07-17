using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Controllers;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;


namespace POSManagementProject.DAL
{
    public class ItemCategoryDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetItemCategorySelectList()
        {
            var list = dbContext.ItemCategories.Where(x => x.ParentId == null).ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public List<ItemCategory> GetItemCategoryList()
        {
            var list = dbContext.ItemCategories.ToList();
            return list;
        }
        public string GetItemCategoryCode()
        {
            long code;

            if (dbContext.ItemCategories.ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.ItemCategories.Select(x => x.Code).Max());

                if (code >= 999)
                {
                    var allNumbers = Enumerable.Range(1, 999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.ItemCategories.Select(x => x.Code))
                        .Select(x => Convert.ToInt64(x));
                    var missingNumbersFirst = allNumbers.Except(currentList).First();
                    code = missingNumbersFirst-1;
                }
            }
            else
            {
                code = 0;
            }

            return string.Format("{0:000}",(code+1));
        }
        public bool IsItemCategorySaved(ItemCategoryVM itemVm)
        {
            ItemCategory item = new ItemCategory()
            {
                Name = itemVm.Name,
                Code = itemVm.Code,
                Description = itemVm.Description,
                Image = itemVm.Image,
                Date = itemVm.Date,
                ParentId = itemVm.ParentId
            };

            dbContext.ItemCategories.Add(item);
            var isAdded = dbContext.SaveChanges() > 0;

            return isAdded;
        }

        public ItemCategoryVM FindItemCategory(long? id)
        {
            var item = dbContext.ItemCategories.Find(id);
            var itemParent = dbContext.ItemCategories.Find(item.ParentId);
            ItemCategoryVM itemVm = null;

            if (item != null)
            {
                itemVm = new ItemCategoryVM()
                {
                    Name = item.Name,
                    Code = item.Code,
                    Description = item.Description,
                    Image = item.Image,
                    Date = item.Date,
                    ParentId = item.ParentId,
                    Parent = itemParent
                };
            }

            return itemVm;
        }

        public bool IsItemCategoryUpdated(ItemCategoryVM itemVm)
        {
            ItemCategory item = new ItemCategory()
            {
                Id = itemVm.Id,
                Name = itemVm.Name,
                Code = itemVm.Code,
                Description = itemVm.Description,
                Image = itemVm.Image,
                Date = itemVm.Date,
                ParentId = itemVm.ParentId
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsItemCategoryDeleted(long id)
        {
            ItemCategory item = dbContext.ItemCategories.Find(id);
            if (item != null)
            {
                dbContext.ItemCategories.Remove(item);
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
            return !dbContext.ItemCategories.Any(x => x.Name == name);
        }

        public bool IsUniqueCode(string code, string initialCode)
        {
            if (initialCode == code)
            {
                return true;
            }
            return !dbContext.ItemCategories.Any(x => x.Code == code);
        }

    }
}