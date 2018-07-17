using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSManagementProject.Controllers
{
    public class BarCodeTestController : Controller
    {
        // GET: BarCodeTest
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string code)
        {
            return View();
        }

        
    }
}