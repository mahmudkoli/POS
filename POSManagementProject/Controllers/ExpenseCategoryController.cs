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
    public class ExpenseCategoryController : Controller
    {
        ExpenseCategoryDAL expenseCategoryDa = new ExpenseCategoryDAL();
        ExpenseCategoryVM ModelVm = new ExpenseCategoryVM();

        // GET: ExpenseCategory
        public ActionResult Index()
        {
            ModelVm.ExpenseCategories = expenseCategoryDa.GetExpenseCategoryList().OrderByDescending(x => x.Date).ToList();
            return View(ModelVm.ExpenseCategories);
        }

        public ActionResult Create()
        {
            ModelVm.SelectList = expenseCategoryDa.GetExpenseCategorySelectList();
            ModelVm.Code = expenseCategoryDa.GetExpenseCategoryCode();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseCategoryVM itemVm)
        {
            itemVm.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (expenseCategoryDa.IsExpenseCategorySaved(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            ModelVm.SelectList = expenseCategoryDa.GetExpenseCategorySelectList();
            ModelVm.Code = expenseCategoryDa.GetExpenseCategoryCode();
            return View(ModelVm);
        }

        public ActionResult Details(long id)
        {
            ExpenseCategoryVM itemVm = expenseCategoryDa.FindExpenseCategory(id);
            return PartialView("_Details", itemVm);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelVm = expenseCategoryDa.FindExpenseCategory(id);

            if (ModelVm == null)
            {
                return HttpNotFound();
            }

            ModelVm.SelectList = expenseCategoryDa.GetExpenseCategorySelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExpenseCategoryVM itemVm)
        {
            if (ModelState.IsValid)
            {
                if (expenseCategoryDa.IsExpenseCategoryUpdated(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(itemVm);
        }

        public ActionResult Delete(long id)
        {
            var isDeleted = expenseCategoryDa.IsExpenseCategoryDeleted(id);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueName(string Name, string initialName)
        {
            var isUnique = expenseCategoryDa.IsUniqueName(Name, initialName);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueCode(string Code, string initialCode)
        {
            var isUnique = expenseCategoryDa.IsUniqueCode(Code, initialCode);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

    }
}