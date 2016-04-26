using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jornalero.web.Controllers
{
    public class HomeController : AuthController
    {
        public ActionResult Dashboard()
        {
            if (!Init())
            return SessionExpiredView();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}