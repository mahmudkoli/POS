using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.DAL
{
    public class PurchaseOperationDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetItemSelectList()
        {
            var list = dbContext.Items.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Code+" - "+li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public IEnumerable<SelectListItem> GetBranchSelectList()
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
        public IEnumerable<SelectListItem> GetSupplierSelectList()
        {
            var list = dbContext.Parties.Where(x => x.IsSupplier == true).ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }

        public List<Employee> GetEmployeeList(long branchId)
        {
            var employeeList = dbContext.Employees.Where(x => x.BranchId == branchId).ToList();
            return employeeList;
        }

        public bool IsPurchaseOperationSuccess(PurchaseOperationInformationVM itemVm)
        {
            PurchaseOperationInformation item = new PurchaseOperationInformation()
            {
                PurchaseNo = itemVm.PurchaseNo,
                BranchId = itemVm.BranchId,
                EmployeeId = itemVm.EmployeeId,
                SupplierId = itemVm.SupplierId,
                PurchaseDate = itemVm.PurchaseDate,
                Remarks = itemVm.Remarks,
                TotalAmount = itemVm.TotalAmount,
                PaidAmount = itemVm.PaidAmount,
                DueAmount = itemVm.DueAmount,
                PurchaseItems = itemVm.PurchaseItems,
                Date = itemVm.Date

            };

            dbContext.PurchaseOperationInformations.Add(item);
            var isSuccess = dbContext.SaveChanges() > 0;

            IsStockUpdated(itemVm.PurchaseItems, itemVm.BranchId);

            return isSuccess;
        }

        public bool IsStockUpdated(List<PurchaseOperationItem> items, long branchId)
        {
            foreach (var item in items)
            {
                var upStockItem = dbContext.Stocks.Where(x => x.ItemId == item.ItemId && x.BranchId == branchId).FirstOrDefault();

                if (upStockItem != null)
                {
                    var newAvgPrice =
                        ((upStockItem.AveragePrice * upStockItem.StockQuantity) + (item.UnitPrice * item.Quantity)) /
                        (upStockItem.StockQuantity + item.Quantity);

                    upStockItem.StockQuantity += item.Quantity;
                    upStockItem.AveragePrice = newAvgPrice;
                }
                else
                {
                    var stockNewItem = new Stock()
                    {
                        BranchId = branchId,
                        ItemId = item.ItemId,
                        StockQuantity = item.Quantity,
                        AveragePrice = item.UnitPrice,
                        Date = DateTime.Now
                    };

                    dbContext.Stocks.Add(stockNewItem);
                }
                
            }

            return dbContext.SaveChanges() > 0;
        }

        public ItemVM GetItem(long id)
        {
            var item = dbContext.Items.Find(id);
            var itemVm = new ItemVM()
            {
                Name = item.Name,
                Code = item.Code,
                CostPrice = item.CostPrice,
                SalePrice = item.SalePrice
            };

            return itemVm;
        }

        public string GetPurchaseNo()
        {
            long purchaseNo;

            if (dbContext.PurchaseOperationInformations.ToList().Count > 0)
            {
                purchaseNo = Convert.ToInt64(dbContext.PurchaseOperationInformations.Select(x => x.PurchaseNo).Max());
            }
            else
            {
                purchaseNo = 0;
            }

            return string.Format("{0:0000000}", (purchaseNo + 1));
        }

        public PurchaseOperationInformation GetPurchaseOpInfo(string purchaseNo)
        {
            PurchaseOperationInformation item = dbContext.PurchaseOperationInformations.Where(x => x.PurchaseNo == purchaseNo).Include(x => x.PurchaseItems).FirstOrDefault();

            return item;
        }

        public PurchaseOperationInformation GetPurchaseOpInfo(long id)
        {
            PurchaseOperationInformation item = dbContext.PurchaseOperationInformations.Where(x => x.Id == id).Include(x => x.PurchaseItems).FirstOrDefault();

            return item;
        }

        public IEnumerable<PurchaseOperationInformation> GetPurchaseOpInfoList()
        {
            var items = dbContext.PurchaseOperationInformations.Include(x => x.PurchaseItems);
            return items;
        }
        public IEnumerable<PurchaseOperationInformation> GetPurchaseOpInfoList(long branchId, DateTime fromDate, DateTime toDate)
        {
            var items = dbContext.PurchaseOperationInformations
                .Where(x => x.BranchId == branchId && x.PurchaseDate >= fromDate && x.PurchaseDate <= toDate)
                .Include(x => x.PurchaseItems);
            return items;
        }
    }
}