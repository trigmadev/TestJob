using Jornalero.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jornalero.web.Helper;


namespace Jornalero.web.Controllers
{
    public class AccountController : AuthController
    {

        // GET: /Acccount/
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string Email, string Password)
        {
            try
            {
                tblSuperAdmin objSuperAdmin = dbJornalero.tblSuperAdmins.Where(x => x.EmailId == Email && x.Password == Password).FirstOrDefault();
                if (objSuperAdmin != null)
                {
                    Session[Constants.UserID]=objSuperAdmin.SuperAdminId;
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failure", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
        }
    }
}