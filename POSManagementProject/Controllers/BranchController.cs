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
    public class BranchController : Controller
    {
        BranchVM ModelVm = new BranchVM();
        BranchDAL branchDa = new BranchDAL();

        // GET: Branch
        public ActionResult Index()
        {
            ModelVm.Branches = branchDa.GetBranchList().OrderByDescending(x => x.Date).ToList();
            return View(ModelVm.Branches);
        }

        public ActionResult Create()
        {
            ModelVm.SelectList = branchDa.GetBranchSelectList();
            ModelVm.Code = branchDa.GetBranchCode();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BranchVM itemVm)
        {
            itemVm.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (branchDa.IsBranchSaved(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            ModelVm.SelectList = branchDa.GetBranchSelectList();
            ModelVm.Code = branchDa.GetBranchCode();
            return View(ModelVm);
        }



        public ActionResult Details(long id)
        {
            BranchVM itemVm = branchDa.FindBranch(id);
            return PartialView("_Details", itemVm);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelVm = branchDa.FindBranch(id);

            if (ModelVm == null)
            {
                return HttpNotFound();
            }

            ModelVm.SelectList = branchDa.GetBranchSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BranchVM itemVm)
        {
            if (ModelState.IsValid)
            {
                if (branchDa.IsBranchUpdated(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(itemVm);
        }

        public ActionResult Delete(long id)
        {
            var isDeleted = branchDa.IsBranchDeleted(id);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueCode(string Code, string initialCode)
        {
            var isUnique = branchDa.IsUniqueCode(Code, initialCode);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }
    }
}