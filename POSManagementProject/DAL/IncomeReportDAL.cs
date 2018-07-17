using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace POSManagementProject.DAL
{
    public class IncomeReportDAL
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
        public IEnumerable<IncomePurchaseReport> GetIncomePurchaseReportList()
        {
            var items = dbContext.PurchaseOperationInformations.Include(x => x.PurchaseItems)
                .GroupBy(x => EntityFunctions.TruncateTime(x.PurchaseDate))
                .Select(x => new IncomePurchaseReport()
                    { PurchaseDate = (DateTime)x.Key, PurchaseCount = x.Count(), PurchaseTotalAmount = x.Sum(y => y.TotalAmount) });
            return items;
        }
        public IEnumerable<IncomeSalesReport> GetIncomeSalesReportList()
        {
            var items = dbContext.SalesOperationInformations.Include(x => x.SalesItems)
                .GroupBy(x => EntityFunctions.TruncateTime(x.SalesDate))
                .Select(x => new IncomeSalesReport()
                    { SalesDate = (DateTime)x.Key, SalesCount = x.Count(), SalesTotalAmount = x.Sum(y => y.TotalAmount) });
            return items;
        }
        public IEnumerable<IncomeExpenseReport> GetIncomeExpenseReportList()
        {
            var items = dbContext.ExpenseOperationInformations.Include(x => x.ExpenseItems)
                .GroupBy(x => EntityFunctions.TruncateTime(x.ExpenseDate))
                .Select(x => new IncomeExpenseReport()
                    { ExpenseDate = (DateTime)x.Key, ExpenseCount = x.Count(), ExpenseTotalAmount = x.Sum(y => y.TotalAmount) });
            return items;
        }

        public IEnumerable<IncomePurchaseReport> GetIncomePurchaseReportList(long branchId, DateTime fromDate, DateTime toDate)
        {
            var items = dbContext.PurchaseOperationInformations.Include(x => x.PurchaseItems)
                .Where(x => x.BranchId == branchId && x.PurchaseDate >= fromDate && x.PurchaseDate <= toDate)
                .GroupBy(x => EntityFunctions.TruncateTime(x.PurchaseDate))
                .Select(x => new IncomePurchaseReport()
                    { PurchaseDate = (DateTime)x.Key, PurchaseCount = x.Count(), PurchaseTotalAmount = x.Sum(y => y.TotalAmount) });
            return items;
        }
        public IEnumerable<IncomeSalesReport> GetIncomeSalesReportList(long branchId, DateTime fromDate, DateTime toDate)
        {
            var items = dbContext.SalesOperationInformations.Include(x => x.SalesItems)
                .Where(x => x.BranchId == branchId && x.SalesDate >= fromDate && x.SalesDate <= toDate)
                .GroupBy(x => EntityFunctions.TruncateTime(x.SalesDate))
                .Select(x => new IncomeSalesReport()
                    { SalesDate = (DateTime)x.Key, SalesCount = x.Count(), SalesTotalAmount = x.Sum(y => y.TotalAmount) });
            return items;
        }
        public IEnumerable<IncomeExpenseReport> GetIncomeExpenseReportList(long branchId, DateTime fromDate, DateTime toDate)
        {
            var items = dbContext.ExpenseOperationInformations.Include(x => x.ExpenseItems)
                .Where(x => x.BranchId == branchId && x.ExpenseDate >= fromDate && x.ExpenseDate <= toDate)
                .GroupBy(x => EntityFunctions.TruncateTime(x.ExpenseDate))
                .Select(x => new IncomeExpenseReport()
                    { ExpenseDate = (DateTime)x.Key, ExpenseCount = x.Count(), ExpenseTotalAmount = x.Sum(y => y.TotalAmount) });
            return items;
        }

        public string GetBranchAddress(long branchId)
        {
            var branchAddress = dbContext.Branches.Where(x => x.Id == branchId).Select(x => x.Address).FirstOrDefault().ToString();
            return branchAddress;
        }
    }
}