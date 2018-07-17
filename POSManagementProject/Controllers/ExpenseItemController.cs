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
    public class ExpenseItemController : Controller
    {
        ExpenseItemDAL expenseItemDa = new ExpenseItemDAL();
        ExpenseItemVM ModelVm = new ExpenseItemVM();

        // GET: ExpenseItem
        public ActionResult Index()
        {
            ModelVm.Items = expenseItemDa.GetExpenseItemList().OrderByDescending(x => x.Date).ToList();
            return View(ModelVm.Items);
        }

        public ActionResult Create()
        {
            ModelVm.SelectList = expenseItemDa.GetExpenseItemSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseItemVM itemVm)
        {
            itemVm.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (expenseItemDa.IsExpenseItemSaved(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            ModelVm.SelectList = expenseItemDa.GetExpenseItemSelectList();
            return View(ModelVm);
        }


        public ActionResult Details(long id)
        {
            ExpenseItemVM itemVm = expenseItemDa.FindExpenseItem(id);
            return PartialView("_Details", itemVm);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelVm = expenseItemDa.FindExpenseItem(id);

            if (ModelVm == null)
            {
                return HttpNotFound();
            }

            ModelVm.SelectList = expenseItemDa.GetExpenseItemSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExpenseItemVM itemVm)
        {
            if (ModelState.IsValid)
            {
                if (expenseItemDa.IsExpenseItemUpdated(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(itemVm);
        }

        public ActionResult Delete(long id)
        {
            var isDeleted = expenseItemDa.IsExpenseItemDeleted(id);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemFullCode(long id)
        {
            var preCode = expenseItemDa.GetExpenseCategoryCode(id);
            var code = expenseItemDa.GetExpenseItemCode(id);
            return Json(new { preCode, code }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueName(string Name, string initialName)
        {
            var isUnique = expenseItemDa.IsUniqueName(Name, initialName);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueCode(string PreCode, string Code, string initialPreCode, string initialCode)
        {
            var isUnique = expenseItemDa.IsUniqueCode(PreCode, Code, initialPreCode, initialCode);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }
    }
}