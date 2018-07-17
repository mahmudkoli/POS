using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.DAL;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeVM ModelVm = new EmployeeVM();
        EmployeeDAL employeeDa = new EmployeeDAL();
        ImageData imageData = new ImageData();

        // GET: Employee
        public ActionResult Index()
        {
            ModelVm.Employees = employeeDa.GetEmployeeList().OrderByDescending(x => x.Date).ToList();
            return View(ModelVm.Employees);
        }

        public ActionResult Create()
        {
            ModelVm.JoiningDate = DateTime.Now;
            ModelVm.Code = employeeDa.GetEmployeeCode();
            ModelVm.SelectListBranch = employeeDa.GetEmployeeBranchSelectList();
            ModelVm.SelectListReference = employeeDa.GetEmployeeSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeVM itemVm,HttpPostedFileBase ItemCategoryFile)
        {
            itemVm.Date = DateTime.Now;
            itemVm.Image = imageData.ImageConvertToByte(ItemCategoryFile);

            if (ModelState.IsValid)
            {
                if (employeeDa.IsEmployeeSaved(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            ModelVm.Code = employeeDa.GetEmployeeCode();
            ModelVm.SelectListBranch = employeeDa.GetEmployeeBranchSelectList();
            ModelVm.SelectListReference = employeeDa.GetEmployeeSelectList();
            return View(ModelVm);
        }




        public ActionResult Details(long id)
        {
            EmployeeVM itemVm = employeeDa.FindEmployee(id);
            return PartialView("_Details", itemVm);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelVm = employeeDa.FindEmployee(id);

            if (ModelVm == null)
            {
                return HttpNotFound();
            }

            ModelVm.SelectListBranch = employeeDa.GetEmployeeBranchSelectList();
            ModelVm.SelectListReference = employeeDa.GetEmployeeSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeVM itemVm)
        {
            if (ModelState.IsValid)
            {
                if (employeeDa.IsEmployeeUpdated(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(itemVm);
        }

        public ActionResult Delete(long id)
        {
            var isDeleted = employeeDa.IsEmployeeDeleted(id);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueName(string Name, string initialName)
        {
            var isUnique = employeeDa.IsUniqueName(Name, initialName);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueCode(string Code, string initialCode)
        {
            var isUnique = employeeDa.IsUniqueCode(Code, initialCode);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

    }
}