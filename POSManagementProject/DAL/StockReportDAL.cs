using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.DAL
{
    public class StockReportDAL
    {
        POSDBContext dbContext = new POSDBContext();
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

        public IEnumerable<Stock> GetStockList()
        {
            var items = dbContext.Stocks.Where(x => x.StockQuantity > 0).Include(x => x.Item).ToList();
            return items;
        }

        public IEnumerable<Stock> GetStockList(long branchId)
        {
            var items = dbContext.Stocks.Where(x => x.BranchId == branchId && x.StockQuantity > 0).Include(x => x.Item).ToList();
            return items;
        }
    }
}