using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;
using System.Web.Mvc;

namespace POSManagementProject.DAL
{
    public class ItemDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetItemSelectList()
        {
            var list = dbContext.ItemCategories.Select(x => x).Distinct().ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public List<Item> GetItemList()
        {
            var list = dbContext.Items.ToList();
            return list;
        }
        public bool IsItemSaved(ItemVM itemVm)
        {
            Item item = new Item()
            {
                Name = itemVm.Name,
                PreCode = itemVm.PreCode,
                Code = itemVm.Code,
                Description = itemVm.Description,
                Image = itemVm.Image,
                Date = itemVm.Date,
                CategoryId = itemVm.CategoryId,
                CostPrice = itemVm.CostPrice,
                SalePrice = itemVm.SalePrice
            };

            dbContext.Items.Add(item);
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

        public string GetItemCategoryCode(long id)
        {
            var code = dbContext.ItemCategories.Where(x => x.Id == id).Select(x => x.Code).FirstOrDefault();
            return code;
        }

        public string GetItemCode(long id)
        {
            long code;
            string preCode = GetItemCategoryCode(id);

            if (dbContext.Items.Where(x => x.PreCode == preCode).ToList().Count > 0)
            {
                code = Convert.ToInt64(dbContext.Items.Where(x => x.PreCode == preCode).Select(x => x.Code).Max());

                if (code >= 999999)
                {
                    var allNumbers = Enumerable.Range(1, 999999).Select(y => (long)y);
                    var currentList = ((IEnumerable<string>)dbContext.Items.Where(x => x.PreCode == preCode).Select(x => x.Code))
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

        public ItemVM FindItem(long? id)
        {
            var item = dbContext.Items.Find(id);
            var itemCategory = dbContext.ItemCategories.Find(item.CategoryId);
            ItemVM itemVm = null;

            if (item != null)
            {
                itemVm = new ItemVM()
                {
                    Name = item.Name,
                    PreCode = item.PreCode,
                    Code = item.Code,
                    CostPrice = item.CostPrice,
                    SalePrice = item.SalePrice,
                    Description = item.Description,
                    Image = item.Image,
                    Date = item.Date,
                    CategoryId = item.CategoryId,
                    Category = itemCategory
                };
            }

            return itemVm;
        }

        public bool IsItemUpdated(ItemVM itemVm)
        {
            Item item = new Item()
            {
                Id = itemVm.Id,
                Name = itemVm.Name,
                PreCode = itemVm.PreCode,
                Code = itemVm.Code,
                CostPrice = itemVm.CostPrice,
                SalePrice = itemVm.SalePrice,
                Description = itemVm.Description,
                Image = itemVm.Image,
                Date = itemVm.Date,
                CategoryId = itemVm.CategoryId
            };

            dbContext.Entry(item).State = EntityState.Modified;
            var isUpdated = dbContext.SaveChanges() > 0;

            return isUpdated;
        }

        public bool IsItemDeleted(long id)
        {
            Item item = dbContext.Items.Find(id);
            if (item != null)
            {
                dbContext.Items.Remove(item);
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
            return !dbContext.Items.Any(x => x.Name == name);
        }

        public bool IsUniqueCode(string preCode, string code, string initialPreCode, string initialCode)
        {
            if ((initialPreCode + initialCode) == (preCode + code))
            {
                return true;
            }
            return !dbContext.Items.Any(x => x.Code == code && x.PreCode == preCode);
        }
    }
}