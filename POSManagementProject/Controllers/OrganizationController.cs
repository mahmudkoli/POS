using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.DAL;
using POSManagementProject.DAL;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.Controllers
{
    public class OrganizationController : Controller
    {
        OrganizationVM ModelVm = new OrganizationVM();
        OrganizationDAL organizationDa = new OrganizationDAL();
        ImageData imageData = new ImageData();

        // GET: Organization
        public ActionResult Index()
        {
            ModelVm.Organizations = organizationDa.GetOrganizationList().OrderByDescending(x => x.Date).ToList();
            return View(ModelVm.Organizations);
        }

        public ActionResult Create()
        {
            ModelVm.Code = organizationDa.GetOrganizationCode();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizationVM itemVm, HttpPostedFileBase ItemCategoryFile)
        {
            itemVm.Date = DateTime.Now;
            itemVm.Image = imageData.ImageConvertToByte(ItemCategoryFile);
            if (ModelState.IsValid)
            {
                if (organizationDa.IsOrganizationSaved(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelVm.Code = organizationDa.GetOrganizationCode();
            return View(ModelVm);
        }


        public ActionResult Details(long id)
        {
            OrganizationVM itemVm = organizationDa.FindOrganization(id);
            return PartialView("_Details", itemVm);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelVm = organizationDa.FindOrganization(id);

            if (ModelVm == null)
            {
                return HttpNotFound();
            }
            
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrganizationVM itemVm)
        {
            if (ModelState.IsValid)
            {
                if (organizationDa.IsOrganizationUpdated(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(itemVm);
        }

        public ActionResult Delete(long id)
        {
            var isDeleted = organizationDa.IsOrganizationDeleted(id);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueName(string Name, string initialName)
        {
            var isUnique = organizationDa.IsUniqueName(Name, initialName);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueCode(string Code, string initialCode)
        {
            var isUnique = organizationDa.IsUniqueCode(Code, initialCode);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

    }
}