using System;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Jornalero.web.Models;
using Jornalero.web.Helper;

namespace Jornalero.web.Controllers
{
    public class AuthController : Controller
    {
        /// <summary> 
        /// DbContext object for the Jornalero  entity model.
        /// </summary>
        protected dbJornaleroEntities dbJornalero = new dbJornaleroEntities();

        /// <summary>
        /// UserID of the logged In User.
        /// </summary>
        //protected int UserID = 22; //{ get { return string.IsNullOrEmpty(Convert.ToString(Session[Constants.UserID]))? 0:Convert.ToInt32(Session[Constants.UserID]); } }
        protected int UserID { get { return string.IsNullOrEmpty(Convert.ToString(Session[Constants.UserID])) ? 0 : Convert.ToInt32(Session[Constants.UserID]); } }
        /// <summary>
        /// UserEmail of the logged In User.
        /// </summary>
        // protected string UserEmail { get { return (Session[Constants.Email].ToString()); } }
        /// <summary>
        /// Username of the currently logged in user.
        /// </summary>
        protected string Username = string.Empty;

        /// <summary>
        /// Initializes the user properties.
        /// </summary>
        /// <returns>True if the user is currently logged in, false otherwise. If the session has expired, false is being returned.</returns>
        protected bool Init()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session[Constants.UserID])) && Convert.ToInt32(Session[Constants.UserID]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// View that can be redirected to if the user session has expired.
        /// </summary>
        /// <returns>Redirection object</returns>
        protected RedirectToRouteResult SessionExpiredView()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Dispose of disposable objects (DbContext).
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //public JsonResult CheckUserExist(string mobile, string email)
        //{
        //    // if user exits than it will return true other false////////
        //    mobile = !string.IsNullOrEmpty(mobile) ? mobile.Trim() : mobile;
        //    email = !string.IsNullOrEmpty(email) ? email.Trim() : email;
        //    ObjectParameter objParam = new ObjectParameter("Status", typeof(int));
        //    dbDermatologist.CheckEmailMobileExist(mobile, email, objParam);
        //    var status = Convert.ToBoolean(objParam.Value);
        //    return Json(status, JsonRequestBehavior.AllowGet);
        //}
    }
}