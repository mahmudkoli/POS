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
    public class ExpenseOperationController : Controller
    {
        ExpenseOperationInformationVM ModelVm = new ExpenseOperationInformationVM();
        ExpenseOperationDAL ExpenseOpDal = new ExpenseOperationDAL();

        // GET: ExpenseOperation
        public ActionResult Index()
        {
            ModelVm.ExpenseDate = DateTime.Now;
            ModelVm.ExpenseNo = ExpenseOpDal.GetExpenseNo();
            ModelVm.SelectListItem = ExpenseOpDal.GetExpenseItemSelectList();
            ModelVm.SelectListBranch = ExpenseOpDal.GetBranchSelectList();
            ModelVm.SelectListEmployee = ExpenseOpDal.GetEmployeeSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Index(ExpenseOperationInformationVM itemVm)
        {
            itemVm.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (ExpenseOpDal.IsExpenseOperationSuccess(itemVm))
                {
                    return RedirectToAction("Result", new { expenseNo = itemVm.ExpenseNo });
                }
            }

            ModelVm.SelectListItem = ExpenseOpDal.GetExpenseItemSelectList();
            ModelVm.SelectListBranch = ExpenseOpDal.GetBranchSelectList();
            ModelVm.SelectListEmployee = ExpenseOpDal.GetEmployeeSelectList();
            return View(ModelVm);
        }

        public ActionResult Result(string expenseNo)
        {
            return View(ExpenseOpDal.GetExpenseOpInfo(expenseNo));
        }

        public ActionResult ExportOneInfoToPdf(string expenseNo)
        {
            ExpenseOperationInfoExportToFile exPdf = new ExpenseOperationInfoExportToFile();
            exPdf.ExportOneInfoToPdf(ExpenseOpDal.GetExpenseOpInfo(expenseNo));
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Report()
        {
            ModelVm.SelectListBranch = ExpenseOpDal.GetBranchSelectList();
            ModelVm.ExpenseOpInfoList = ExpenseOpDal.GetExpenseOpInfoList();
            return View(ModelVm);
        }

        [HttpPost]
        public ActionResult Report(long branchId, DateTime fromDate, DateTime toDate)
        {
            ModelVm.SelectListBranch = ExpenseOpDal.GetBranchSelectList();
            ModelVm.ExpenseOpInfoList = ExpenseOpDal.GetExpenseOpInfoList(branchId, fromDate, toDate);
            return View(ModelVm);
        }

        public ActionResult ExpenseOpReportDetails(long id)
        {
            var item = ExpenseOpDal.GetExpenseOpInfo(id);
            return PartialView("_ExpenseOpReportDetails", item);
        }

        public ActionResult ExportAllInfoToPdf(long branchId, DateTime fromDate, DateTime toDate)
        {
            ExpenseOperationInfoExportToFile exPdf = new ExpenseOperationInfoExportToFile();
            exPdf.ExportAllInfoToPdf(ExpenseOpDal.GetExpenseOpInfoList(branchId, fromDate, toDate), fromDate, toDate);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeList(long branchId)
        {
            var employeeList = ExpenseOpDal.GetEmployeeList(branchId);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

    }
}