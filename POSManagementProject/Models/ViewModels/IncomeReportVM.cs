using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.Models.ViewModels
{
    public class IncomeReportVM
    {
        public IEnumerable<SelectListItem> SelectListBranch { get; set; }
        public IEnumerable<IncomePurchaseReport> IncomePurchaseReportList { get; set; }
        public IEnumerable<IncomeSalesReport> IncomeSalesReportList { get; set; }
        public IEnumerable<IncomeExpenseReport> IncomeExpenseReportList { get; set; }
    }

    public class IncomePurchaseReport
    {
        public DateTime PurchaseDate { get; set; }
        public int PurchaseCount { get; set; }
        public double PurchaseTotalAmount { get; set; }
    }

    public class IncomeSalesReport
    {
        public DateTime SalesDate { get; set; }
        public int SalesCount { get; set; }
        public double SalesTotalAmount { get; set; }
    }

    public class IncomeExpenseReport
    {
        public DateTime ExpenseDate { get; set; }
        public int ExpenseCount { get; set; }
        public double ExpenseTotalAmount { get; set; }
    }

}