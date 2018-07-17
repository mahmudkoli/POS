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
    public class SalesOperationDAL
    {

        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetItemSelectList()
        {
            var list = dbContext.Items.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Code + " - " + li.Name, Value = li.Id.ToString() }));

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

        public List<Employee> GetEmployeeList(long branchId)
        {
            var employeeList = dbContext.Employees.Where(x => x.BranchId == branchId).ToList();
            return employeeList;
        }
        public List<Item> GetStockItemList(long branchId)
        {
            var stockList = dbContext.Stocks.Where(x => x.BranchId == branchId && x.StockQuantity > 0).Include(x => x.Item).Select(x => x.Item).ToList();
            return stockList;
        }

        public string GetSalesNo()
        {
            long salesNo;

            if (dbContext.SalesOperationInformations.ToList().Count > 0)
            {
                salesNo = Convert.ToInt64(dbContext.SalesOperationInformations.Select(x => x.SalesNo).Max());
            }
            else
            {
                salesNo = 0;
            }

            return string.Format("{0:0000000}", (salesNo + 1));
        }

        public bool IsSalesOperationSuccess(SalesOperationInformationVM itemVm)
        {
            SalesOperationInformation item = new SalesOperationInformation()
            {
                SalesNo = itemVm.SalesNo,
                BranchId = itemVm.BranchId,
                EmployeeId = itemVm.EmployeeId,
                SalesDate = itemVm.SalesDate,
                CustomerName = itemVm.CustomerName,
                CustomerContact = itemVm.CustomerContact,
                TotalAmount = itemVm.TotalAmount,
                VAT = itemVm.VAT,
                DiscountAmount = itemVm.DiscountAmount,
                PayableAmount = itemVm.PayableAmount,
                PaidAmount = itemVm.PaidAmount,
                DueAmount = itemVm.DueAmount,
                SalesItems = itemVm.SalesItems,
                Date = itemVm.Date

            };

            dbContext.SalesOperationInformations.Add(item);
            var isSuccess = dbContext.SaveChanges() > 0;

            IsStockUpdated(itemVm.SalesItems, itemVm.BranchId);

            return isSuccess;
        }

        public bool IsStockUpdated(List<SalesOperationItem> items, long branchId)
        {
            foreach (var item in items)
            {
                var upStockItem = dbContext.Stocks.Where(x => x.ItemId == item.ItemId && x.BranchId == branchId).FirstOrDefault();

                if (upStockItem != null)
                {
                    upStockItem.StockQuantity -= item.Quantity;
                }
            }

            return dbContext.SaveChanges() > 0;
        }

        public object GetItemStatus(long branchId, long itemId)
        {
            var stockQty = dbContext.Stocks.Where(x => x.BranchId == branchId && x.ItemId == itemId).Select(x => x.StockQuantity).FirstOrDefault();
            var salesPrice = dbContext.Items.Where(x => x.Id == itemId).Select(x => x.SalePrice).FirstOrDefault();
            
            long itemStockQty = 0;
            double itemSalesPrice = 0;

            if (stockQty != null)
                itemStockQty = stockQty;
            if (salesPrice != null)
                itemSalesPrice = salesPrice;

            var obj = new { ItemStockQty = itemStockQty, ItemSalesPrice = itemSalesPrice };

            return obj;
        }

        public SalesOperationInformation GetSalesOpInfo(string salesNo)
        {
            SalesOperationInformation item = dbContext.SalesOperationInformations.Where(x => x.SalesNo == salesNo).Include(x => x.SalesItems).FirstOrDefault();

            return item;
        }

        public SalesOperationInformation GetSalesOpInfo(long id)
        {
            SalesOperationInformation item = dbContext.SalesOperationInformations.Where(x => x.Id == id).Include(x => x.SalesItems).FirstOrDefault();

            return item;
        }

        public IEnumerable<SalesOperationInformation> GetSalesOpInfoList()
        {
            var items = dbContext.SalesOperationInformations.Include(x => x.SalesItems);
            return items;
        }
        public IEnumerable<SalesOperationInformation> GetSalesOpInfoList(long branchId, DateTime fromDate, DateTime toDate)
        {
            var items = dbContext.SalesOperationInformations
                .Where(x => x.BranchId == branchId && x.SalesDate >= fromDate && x.SalesDate <= toDate)
                .Include(x => x.SalesItems);
            return items;
        }

    }
}