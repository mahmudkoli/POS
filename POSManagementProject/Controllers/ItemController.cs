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
    public class ItemController : Controller
    {
        ItemVM ModelVm = new ItemVM();
        ItemDAL itemDa = new ItemDAL();
        ImageData imageData = new ImageData();

        // GET: Item
        public ActionResult Index()
        {
            ModelVm.Items = itemDa.GetItemList().OrderByDescending(x => x.Date).ToList();
            return View(ModelVm.Items);
        }

        public ActionResult Create()
        {
            ModelVm.SelectList = itemDa.GetItemSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemVM itemVm, HttpPostedFileBase ItemCategoryFile)
        {
            itemVm.Date = DateTime.Now;
            itemVm.Image = imageData.ImageConvertToByte(ItemCategoryFile);

            if (ModelState.IsValid)
            {
                if (itemDa.IsItemSaved(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            ModelVm.SelectList = itemDa.GetItemSelectList();
            return View(ModelVm);
        }



        public ActionResult Details(long id)
        {
            ItemVM itemVm = itemDa.FindItem(id);
            return PartialView("_Details", itemVm);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelVm = itemDa.FindItem(id);

            if (ModelVm == null)
            {
                return HttpNotFound();
            }

            ModelVm.SelectList = itemDa.GetItemSelectList();
            return View(ModelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemVM itemVm)
        {
            if (ModelState.IsValid)
            {
                if (itemDa.IsItemUpdated(itemVm))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(itemVm);
        }

        public ActionResult Delete(long id)
        {
            var isDeleted = itemDa.IsItemDeleted(id);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemFullCode(long id)
        {
            var preCode = itemDa.GetItemCategoryCode(id);
            var code = itemDa.GetItemCode(id);
            return Json(new{preCode, code}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueName(string Name, string initialName)
        {
            var isUnique = itemDa.IsUniqueName(Name, initialName);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueCode(string PreCode, string Code, string initialPreCode, string initialCode)
        {
            var isUnique = itemDa.IsUniqueCode(PreCode, Code, initialPreCode, initialCode);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

    }
}