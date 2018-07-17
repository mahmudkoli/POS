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
    public class IncomeReportController : Controller
    {
        IncomeReportVM ModelVm = new IncomeReportVM();
        IncomeReportDAL incomeReportDal = new IncomeReportDAL();

        // GET: IncomeReport
        public ActionResult Index()
        {
            ModelVm.SelectListBranch = incomeReportDal.GetBranchSelectList();
            ModelVm.IncomePurchaseReportList = incomeReportDal.GetIncomePurchaseReportList();
            ModelVm.IncomeSalesReportList = incomeReportDal.GetIncomeSalesReportList();
            ModelVm.IncomeExpenseReportList = incomeReportDal.GetIncomeExpenseReportList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Index(long branchId, DateTime fromDate, DateTime toDate)
        {
            ModelVm.SelectListBranch = incomeReportDal.GetBranchSelectList();
            ModelVm.IncomePurchaseReportList = incomeReportDal.GetIncomePurchaseReportList(branchId, fromDate, toDate);
            ModelVm.IncomeSalesReportList = incomeReportDal.GetIncomeSalesReportList(branchId, fromDate, toDate);
            ModelVm.IncomeExpenseReportList = incomeReportDal.GetIncomeExpenseReportList(branchId, fromDate, toDate);
            return View(ModelVm);
        }

        public ActionResult ExportAllInfoToPdf(long branchId, DateTime fromDate, DateTime toDate)
        {
            IncomeInfoExportToFile exPdf = new IncomeInfoExportToFile();
            exPdf.ExportAllInfoToPdf(incomeReportDal.GetIncomePurchaseReportList(branchId, fromDate, toDate),
                incomeReportDal.GetIncomeSalesReportList(branchId, fromDate, toDate),
                incomeReportDal.GetIncomeExpenseReportList(branchId, fromDate, toDate),
                incomeReportDal.GetBranchAddress(branchId), fromDate, toDate);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}