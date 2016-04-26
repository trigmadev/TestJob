using Jornalero.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jornalero.web.Controllers
{

    public class SuperAdminController : AuthController
    {


        //
        // GET: /SuperAdmin/

        public ActionResult Dashboard()
        {
          if (!Init())
                return SessionExpiredView();
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddProfession()
        {
            if (!Init())
                return SessionExpiredView();
            return View();
        }
        public ActionResult ProfessionList()
        {
            try
            {
                if (!Init())
                    return SessionExpiredView();
                List<tblProfession> ObjProfessionList = dbJornalero.tblProfessions.Where(x => x.IsActive == true).ToList();
                if (ObjProfessionList.Count > 0)
                {
                    return View(ObjProfessionList);
                }
                else
                {
                    return View(ObjProfessionList);

                }
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddProfession(tblProfession ObjProfession)
        {
            try
            {
                if (!Init())
                    return SessionExpiredView();

                if (!string.IsNullOrEmpty(ObjProfession.Name))
                {
                    tblProfession ObjProfession1 = dbJornalero.tblProfessions.Where(X => X.Name == ObjProfession.Name && X.IsActive == true).FirstOrDefault();
                    if (ObjProfession1 == null)
                    {
                        ObjProfession.Name = ObjProfession.Name;
                        ObjProfession.IsActive = true;
                        ObjProfession.CreatedDate = System.DateTime.UtcNow;
                        ObjProfession.ModifiedDate = System.DateTime.UtcNow;
                        dbJornalero.tblProfessions.Add(ObjProfession);
                        dbJornalero.SaveChanges();
                        return RedirectToAction("ProfessionList");
                    }
                    else
                    {
                        ViewBag.Message = "This Profession Is Already Exist";
                        return View(ObjProfession);
                    }
                }
                else
                {
                    ViewBag.Message = "Please Enter Profession Name";
                    return View();
                }

            }
            catch (Exception)
            {
                ViewBag.Message = "Something Going Wrong While Saving Records";
                return View();

            }
        }
        [HttpGet]
        public ActionResult EditProfession(int ProfessionId)
        {
            try
            {
                if (!Init())
                    return SessionExpiredView();
                tblProfession ObjProfession = dbJornalero.tblProfessions.Where(x => x.ProfessionId == ProfessionId).FirstOrDefault();
                if (ObjProfession != null)
                {

                    return View(ObjProfession);
                }
                else
                {
                 return RedirectToAction("ProfessionList");
                }
            }
            catch (Exception)
            {

                return View();

            }
        }
        [HttpPost]
        public ActionResult EditProfession(tblProfession ObjProfession)
        {
            try
            {
                if (!Init())
                    return SessionExpiredView();

                if (!string.IsNullOrEmpty(ObjProfession.Name))
                {
                    tblProfession ObjProfession1 = dbJornalero.tblProfessions.Where(x => x.ProfessionId == ObjProfession.ProfessionId).FirstOrDefault();
                    if (ObjProfession != null)
                    {
                        tblProfession ObjProfession2 = dbJornalero.tblProfessions.Where(x => x.ProfessionId != ObjProfession.ProfessionId && x.Name == ObjProfession.Name && x.IsActive == true).FirstOrDefault();
                        if (ObjProfession2 == null)
                        {
                            ObjProfession1.Name = ObjProfession.Name;
                            ObjProfession1.ModifiedDate = System.DateTime.UtcNow;
                            dbJornalero.tblProfessions.Attach(ObjProfession1);
                            var entry = dbJornalero.Entry(ObjProfession1);
                            entry.Property(x => x.Name).IsModified = true;
                            entry.Property(x => x.ModifiedDate).IsModified = true;
                            dbJornalero.SaveChanges();
                            return RedirectToAction("ProfessionList");
                        }
                        else
                        {
                            ViewBag.Message = "This Profession Is Already Exist";
                            return View(ObjProfession);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Record Does not Exist";
                        return View(ObjProfession);
                    }
                }
                else
                {
                    ViewBag.Message = "Please Enter Profession Name";
                    return View(ObjProfession);

                }

            }
            catch (Exception)
            {

                ViewBag.Message = "Something Going Wrong While Updating Records";
                return View();

            }
        }

        public JsonResult DeleteProfession(int ProfessionId)
        {
            try
            {
                tblProfession ObjProfession = dbJornalero.tblProfessions.Where(x => x.ProfessionId == ProfessionId).FirstOrDefault();
                if (ObjProfession != null)
                {

                    ObjProfession.IsActive = false;
                    dbJornalero.tblProfessions.Attach(ObjProfession);
                    var entry = dbJornalero.Entry(ObjProfession);
                    entry.Property(x => x.IsActive).IsModified = true;
                    dbJornalero.SaveChanges();
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
        [HttpGet]
        public ActionResult AddArea()
        {
            return View();

        }
        [HttpPost]
        public ActionResult AddArea(tblArea ObjArea)
        {
            try
            {

                if (!Init())
                    return SessionExpiredView();
                if (!string.IsNullOrEmpty(ObjArea.Name))
                {
                    tblProfession ObjArea1 = dbJornalero.tblProfessions.Where(X => X.Name == ObjArea.Name && X.IsActive == true).FirstOrDefault();
                    if (ObjArea1 == null)
                    {
                        ObjArea.Name = ObjArea.Name;
                        ObjArea.IsActive = true;
                        ObjArea.CreatedDate = System.DateTime.UtcNow;
                        ObjArea.Modifieddate = System.DateTime.UtcNow;
                        dbJornalero.tblAreas.Add(ObjArea);
                        dbJornalero.SaveChanges();
                        return RedirectToAction("AreaList");
                    }
                    else
                    {
                        ViewBag.Message = "This Area Is Already Exist";
                        return View(ObjArea);

                    }

                }
                else
                {
                    ViewBag.Message = "Please Enter Area Name";
                    return View();
                }

            }
            catch (Exception)
            {
                ViewBag.Message = "Something Going Wrong While Saving Records";
                return View();

            }
        }
        [HttpGet]
        public ActionResult EditArea(int AreaId)
        {
            try
            {
                if (!Init())
                    return SessionExpiredView();
                tblArea ObjArea = dbJornalero.tblAreas.Where(x => x.AreaId == AreaId).FirstOrDefault();
                if (ObjArea != null)
                {

                    return View(ObjArea);
                }
                else
                {
                return RedirectToAction("AreaList");
                }
            }
            catch (Exception)
            {

                return View();

            }
        }
        [HttpPost]
        public ActionResult EditArea(tblArea ObjArea)
        {
            try
            {    if (!Init())
                    return SessionExpiredView();
                if (!string.IsNullOrEmpty(ObjArea.Name))
                {
                    tblArea ObjArea1 = dbJornalero.tblAreas.Where(x => x.AreaId == ObjArea.AreaId).FirstOrDefault();
                    if (ObjArea1 != null)
                    {
                        tblArea ObjArea2 = dbJornalero.tblAreas.Where(x => x.AreaId != ObjArea.AreaId && x.Name == ObjArea.Name && x.IsActive == true).FirstOrDefault();
                        if (ObjArea2 == null)
                        {
                            ObjArea1.Name = ObjArea.Name;
                            ObjArea1.Modifieddate = System.DateTime.UtcNow;
                            dbJornalero.tblAreas.Attach(ObjArea1);
                            var entry = dbJornalero.Entry(ObjArea1);
                            entry.Property(x => x.Name).IsModified = true;
                            entry.Property(x => x.Modifieddate).IsModified = true;
                            dbJornalero.SaveChanges();
                            return RedirectToAction("AreaList");
                        }
                        else
                        {
                            ViewBag.Message = "This Area Is Already Exist";
                            return View(ObjArea);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Record Does not Exist";
                        return View(ObjArea);
                    }
                }
                else
                {
                    ViewBag.Message = "Please Enter Area Name";
                    return View(ObjArea);

                }

            }
            catch (Exception)
            {

                ViewBag.Message = "Something Going Wrong While Updating Records";
                return View();

            }


        }
        public ActionResult AreaList()
        {

            try
            {

                if (!Init())
                    return SessionExpiredView();
                List<tblArea> ObjAreaList = dbJornalero.tblAreas.Where(x => x.IsActive == true).ToList();
                if (ObjAreaList.Count > 0)
                {
                    return View(ObjAreaList);
                }
                else
                {
                    return View(ObjAreaList);

                }

            }
            catch (Exception)
            {

                return View();
                //Something Going Wrong while Fetching Records;
            }

        }
        public JsonResult DeleteArea(int AreaId)
        {
            try
            {
                tblArea ObjProfession = dbJornalero.tblAreas.Where(x => x.AreaId == AreaId).FirstOrDefault();
                if (ObjProfession != null)
                {

                    ObjProfession.IsActive = false;
                    dbJornalero.tblAreas.Attach(ObjProfession);
                    var entry = dbJornalero.Entry(ObjProfession);
                    entry.Property(x => x.IsActive).IsModified = true;
                    dbJornalero.SaveChanges();
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
        [HttpGet]
        public ActionResult AddBirthPlace()
        {

            if (!Init())
                return SessionExpiredView();
            return View();

        }
        [HttpPost]
        public ActionResult AddBirthPlace(tblBirthPlace ObjBirthPlace)
        {
            try
            {
                if (!Init())
                    return SessionExpiredView();
                if (!string.IsNullOrEmpty(ObjBirthPlace.Name))
                {
                    tblBirthPlace ObjBirthPlace1 = dbJornalero.tblBirthPlaces.Where(X => X.Name == ObjBirthPlace.Name && X.IsActive == true).FirstOrDefault();
                    if (ObjBirthPlace1 == null)
                    {
                        ObjBirthPlace.Name = ObjBirthPlace.Name;
                        ObjBirthPlace.IsActive = true;
                        ObjBirthPlace.CreatedDate = System.DateTime.UtcNow;
                        ObjBirthPlace.ModifiedDate = System.DateTime.UtcNow;
                        dbJornalero.tblBirthPlaces.Add(ObjBirthPlace);
                        dbJornalero.SaveChanges();
                        return RedirectToAction("BirthPlaceList");
                    }
                    else
                    {
                        ViewBag.Message = "This Birth Place Is Already Exist";
                        return View(ObjBirthPlace);

                    }

                }
                else
                {
                    ViewBag.Message = "Please Enter Birth Place Name";
                    return View();
                }

            }
            catch (Exception)
            {
                ViewBag.Message = "Something Going Wrong While Saving Records";
                return View();

            }


        }
        [HttpGet]
        public ActionResult EditBirthPlace(int BirthPlaceID)
        {

            try
            {

                if (!Init())
                    return SessionExpiredView();
                tblBirthPlace ObjBirthPlace = dbJornalero.tblBirthPlaces.Where(x => x.BirthPlaceID == BirthPlaceID).FirstOrDefault();
                if (ObjBirthPlace != null)
                {

                    return View(ObjBirthPlace);
                }
                else
                {

                    return RedirectToAction("BirthPlaceList");
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return View();

            }

        }
        public ActionResult EditBirthPlace(tblBirthPlace ObjBirthPlace)
        {

            try
            {
                if (!Init())
                    return SessionExpiredView();
                if (!string.IsNullOrEmpty(ObjBirthPlace.Name))
                {
                    tblBirthPlace ObjBirthPlace1 = dbJornalero.tblBirthPlaces.Where(x => x.BirthPlaceID == ObjBirthPlace.BirthPlaceID).FirstOrDefault();
                    if (ObjBirthPlace1 != null)
                    {
                        tblBirthPlace ObjBirthPlace2 = dbJornalero.tblBirthPlaces.Where(x => x.BirthPlaceID != ObjBirthPlace.BirthPlaceID && x.Name == ObjBirthPlace.Name && x.IsActive == true).FirstOrDefault();
                        if (ObjBirthPlace2 == null)
                        {
                            ObjBirthPlace1.Name = ObjBirthPlace.Name;
                            ObjBirthPlace1.ModifiedDate = System.DateTime.UtcNow;
                            dbJornalero.tblBirthPlaces.Attach(ObjBirthPlace1);
                            var entry = dbJornalero.Entry(ObjBirthPlace1);
                            entry.Property(x => x.Name).IsModified = true;
                            entry.Property(x => x.ModifiedDate).IsModified = true;
                            dbJornalero.SaveChanges();
                            return RedirectToAction("BirthPlaceList");
                        }
                        else
                        {
                            ViewBag.Message = "This Birth Place Is Already Exist";
                            return View(ObjBirthPlace);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Record Does not Exist";
                        return View(ObjBirthPlace);
                    }
                }
                else
                {
                    ViewBag.Message = "Please Enter Area Name";
                    return View(ObjBirthPlace);

                }

            }
            catch (Exception ex)
            {

                ViewBag.Message = "Something Going Wrong While Updating Records";
                ex.Message.ToString();
                return View();

            }


        }
        public JsonResult DeleteBirthPlace(int BirthPlaceId)
        {
            try
            {

                tblBirthPlace ObjBirthPlace = dbJornalero.tblBirthPlaces.Where(x => x.BirthPlaceID == BirthPlaceId).FirstOrDefault();
                if (ObjBirthPlace != null)
                {

                    ObjBirthPlace.IsActive = false;
                    dbJornalero.tblBirthPlaces.Attach(ObjBirthPlace);
                    var entry = dbJornalero.Entry(ObjBirthPlace);
                    entry.Property(x => x.IsActive).IsModified = true;
                    dbJornalero.SaveChanges();
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


        public ActionResult BirthPlaceList()
        {

            try
            {
                if (!Init())
                    return SessionExpiredView();
                List<tblBirthPlace> ObjBirthPlaceListList = dbJornalero.tblBirthPlaces.Where(x => x.IsActive == true).ToList();
                if (ObjBirthPlaceListList.Count > 0)
                {
                    return View(ObjBirthPlaceListList);
                }
                else
                {
                    return View(ObjBirthPlaceListList);

                }

            }
            catch (Exception)
            {

                return View();
                //Something Going Wrong while Fetching Records;
            }

        }



    }
}