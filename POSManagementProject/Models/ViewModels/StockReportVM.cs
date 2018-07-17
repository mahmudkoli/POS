using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.Models.ViewModels
{
    public class StockReportVM
    {
        public IEnumerable<SelectListItem> SelectListBranch { get; set; }
        public IEnumerable<Stock> StockList { get; set; }
    }
}