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
    public class SalesOperationController : Controller
    {
        SalesOperationInformationVM ModelVm = new SalesOperationInformationVM();
        SalesOperationDAL SalesOpDal = new SalesOperationDAL();

        // GET: SalesOperation
        public ActionResult Index()
        {
            ModelVm.SalesDate = DateTime.Now;
            ModelVm.SalesNo = SalesOpDal.GetSalesNo();
            ModelVm.SelectListItem = SalesOpDal.GetItemSelectList();
            ModelVm.SelectListBranch = SalesOpDal.GetBranchSelectList();
            ModelVm.SelectListEmployee = SalesOpDal.GetEmployeeSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Index(SalesOperationInformationVM itemVm)
        {
            itemVm.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (SalesOpDal.IsSalesOperationSuccess(itemVm))
                {
                    return RedirectToAction("Result",new {salesNo = itemVm.SalesNo});
                }
            }

            ModelVm.SelectListItem = SalesOpDal.GetItemSelectList();
            ModelVm.SelectListBranch = SalesOpDal.GetBranchSelectList();
            ModelVm.SelectListEmployee = SalesOpDal.GetEmployeeSelectList();
            return View(ModelVm);
        }

        public ActionResult GetItemStatus(long branchId, long itemId)
        {
            var obj = SalesOpDal.GetItemStatus(branchId, itemId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Result(string salesNo)
        {
            return View(SalesOpDal.GetSalesOpInfo(salesNo));
        }

        public ActionResult ExportOneInfoToPdf(string salesNo)
        {
            SalesOperationInfoExportToFile exPdf = new SalesOperationInfoExportToFile();
            exPdf.ExportOneInfoToPdf(SalesOpDal.GetSalesOpInfo(salesNo));
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            ModelVm.SelectListBranch = SalesOpDal.GetBranchSelectList();
            ModelVm.SalesOpInfoList = SalesOpDal.GetSalesOpInfoList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Report(long branchId, DateTime fromDate, DateTime toDate)
        {
            ModelVm.SelectListBranch = SalesOpDal.GetBranchSelectList();
            ModelVm.SalesOpInfoList = SalesOpDal.GetSalesOpInfoList(branchId, fromDate, toDate);
            return View(ModelVm);
        }

        public ActionResult SalesOpReportDetails(long id)
        {
            var item = SalesOpDal.GetSalesOpInfo(id);
            return PartialView("_SalesOpReportDetails", item);
        }

        public ActionResult ExportAllInfoToPdf(long branchId, DateTime fromDate, DateTime toDate)
        {
            SalesOperationInfoExportToFile exPdf = new SalesOperationInfoExportToFile();
            exPdf.ExportAllInfoToPdf(SalesOpDal.GetSalesOpInfoList(branchId, fromDate, toDate), fromDate, toDate);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeList(long branchId)
        {
            var employeeList = SalesOpDal.GetEmployeeList(branchId);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStockItemList(long branchId)
        {
            var employeeList = SalesOpDal.GetStockItemList(branchId);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

    }
}