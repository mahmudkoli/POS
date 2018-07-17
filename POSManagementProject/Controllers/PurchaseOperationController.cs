using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.DAL;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;
using POSManagementProject.Report;

namespace POSManagementProject.Controllers
{
    public class PurchaseOperationController : Controller
    {
        PurchaseOperationInformationVM ModelVm = new PurchaseOperationInformationVM();
        PurchaseOperationDAL PurchaseOpDal = new PurchaseOperationDAL();

        // GET: PurchaseOperation
        public ActionResult Index()
        {
            ModelVm.PurchaseDate = DateTime.Now;
            ModelVm.PurchaseNo = PurchaseOpDal.GetPurchaseNo();
            ModelVm.SelectListItem = PurchaseOpDal.GetItemSelectList();
            ModelVm.SelectListBranch = PurchaseOpDal.GetBranchSelectList();
            ModelVm.SelectListEmployee = PurchaseOpDal.GetEmployeeSelectList();
            ModelVm.SelectListSupplier = PurchaseOpDal.GetSupplierSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Index(PurchaseOperationInformationVM itemVm)
        {
            itemVm.Date = DateTime.Now;

            if(ModelState.IsValid)
            {
                if (PurchaseOpDal.IsPurchaseOperationSuccess(itemVm))
                {
                    return RedirectToAction("Result", new { purchaseNo = itemVm.PurchaseNo });
                }
            }

            ModelVm.SelectListItem = PurchaseOpDal.GetItemSelectList();
            ModelVm.SelectListBranch = PurchaseOpDal.GetBranchSelectList();
            ModelVm.SelectListEmployee = PurchaseOpDal.GetEmployeeSelectList();
            ModelVm.SelectListSupplier = PurchaseOpDal.GetSupplierSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult GetItem(long id)
        {
            var getItem = PurchaseOpDal.GetItem(id);
            return Json(getItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Result(string purchaseNo)
        {
            return View(PurchaseOpDal.GetPurchaseOpInfo(purchaseNo));
        }

        public ActionResult ExportOneInfoToPdf(string purchaseNo)
        {
            PurchaseOperationInfoExportToFile exPdf = new PurchaseOperationInfoExportToFile();
            exPdf.ExportOneInfoToPdf(PurchaseOpDal.GetPurchaseOpInfo(purchaseNo));
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            ModelVm.SelectListBranch = PurchaseOpDal.GetBranchSelectList();
            ModelVm.PurchaseOpInfoList = PurchaseOpDal.GetPurchaseOpInfoList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Report(long branchId, DateTime fromDate, DateTime toDate)
        {
            ModelVm.SelectListBranch = PurchaseOpDal.GetBranchSelectList();
            ModelVm.PurchaseOpInfoList = PurchaseOpDal.GetPurchaseOpInfoList(branchId,fromDate,toDate);
            return View(ModelVm);
        }

        public ActionResult PurchaseOpReportDetails(long id)
        {
            var item = PurchaseOpDal.GetPurchaseOpInfo(id);
            return PartialView("_PurchaseOpReportDetails",item);
        }

        public ActionResult ExportAllInfoToPdf(long branchId, DateTime fromDate, DateTime toDate)
        {
            PurchaseOperationInfoExportToFile exPdf = new PurchaseOperationInfoExportToFile();
            exPdf.ExportAllInfoToPdf(PurchaseOpDal.GetPurchaseOpInfoList(branchId, fromDate, toDate), fromDate, toDate);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeList(long branchId)
        {
            var employeeList = PurchaseOpDal.GetEmployeeList(branchId);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

    }
}