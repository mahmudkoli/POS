using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.DAL;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.Controllers
{
    public class PartyController : Controller
    {
        PartyVM ModelVm = new PartyVM();
        PartyDAL partyDa = new PartyDAL();
        ImageData imageData = new ImageData();

        // GET: Party
        public ActionResult Index()
        {
            ModelVm.Parties = partyDa.GetPartyList().OrderByDescending(x => x.Date).ToList();
            return View(ModelVm.Parties);
        }

        public ActionResult Create()
        {
            ModelVm.PreCode = partyDa.GetPartyPreCode();
            ModelVm.Code = partyDa.GetPartyCode();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PartyVM itemVm, HttpPostedFileBase ItemCategoryFile)
        {
            itemVm.Date = DateTime.Now;
            itemVm.Image = imageData.ImageConvertToByte(ItemCategoryFile);

            if (ModelState.IsValid)
            {
                if (partyDa.IsPartySaved(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            ModelVm.PreCode = partyDa.GetPartyPreCode();
            ModelVm.Code = partyDa.GetPartyCode();
            return View(ModelVm);
        }


        public ActionResult Details(long id)
        {
            PartyVM itemVm = partyDa.FindParty(id);
            return PartialView("_Details", itemVm);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelVm = partyDa.FindParty(id);

            if (ModelVm == null)
            {
                return HttpNotFound();
            }
            
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartyVM itemVm)
        {
            if (ModelState.IsValid)
            {
                if (partyDa.IsPartyUpdated(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(itemVm);
        }

        public ActionResult Delete(long id)
        {
            var isDeleted = partyDa.IsPartyDeleted(id);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueName(string Name, string initialName)
        {
            var isUnique = partyDa.IsUniqueName(Name, initialName);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueCode(string PreCode, string Code, string initialPreCode, string initialCode)
        {
            var isUnique = partyDa.IsUniqueCode(PreCode, Code, initialPreCode, initialCode);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }
    }
}