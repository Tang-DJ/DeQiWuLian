using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeQiWuLiang.Controllers
{
    public class WeUITestController : Controller
    {
        // GET: WeUITest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestView()
        {
            return View("TestView");
        }
    }
}