using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSManagementProject.Report
{
    public class FileSaveLocation
    {
        private string Location = @"C:\Users\mahmu\Desktop\Report\";

        public string SalesReportPdfLocation
        {
            get { return Location + @"SalesReport\"; }
        }

        public string PurchaseReportPdfLocation
        {
            get { return Location + @"PurchaseReport\"; }
        }

        public string ExpenseReportPdfLocation
        {
            get { return Location + @"ExpenseReport\"; }
        }

        public string IncomeReportPdfLocation
        {
            get { return Location + @"IncomeReport\"; }
        }

        public string StockReportPdfLocation
        {
            get { return Location + @"StockReport\"; }
        }
    }
}