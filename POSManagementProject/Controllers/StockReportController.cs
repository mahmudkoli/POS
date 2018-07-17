using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.DAL;
using POSManagementProject.Models.ViewModels;
using POSManagementProject.Report;

namespace POSManagementProject.Controllers
{
    public class StockReportController : Controller
    {
        StockReportVM ModelVm = new StockReportVM();
        StockReportDAL stockReportDal = new StockReportDAL();

        // GET: StockReport
        public ActionResult Index()
        {
            ModelVm.SelectListBranch = stockReportDal.GetBranchSelectList();
            ModelVm.StockList = stockReportDal.GetStockList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Index(long branchId)
        {
            ModelVm.SelectListBranch = stockReportDal.GetBranchSelectList();
            ModelVm.StockList = stockReportDal.GetStockList(branchId);
            return View(ModelVm);
        }


        public ActionResult ExportAllInfoToPdf(long branchId)
        {
            StockInfoExportToFile exPdf = new StockInfoExportToFile();
            exPdf.ExportAllInfoToPdf(stockReportDal.GetStockList(branchId));
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}