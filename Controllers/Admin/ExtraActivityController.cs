using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OHD_System.Models.Entities;
using OHD_System.Models.Repositories;
using System.Web.UI;
using System.Runtime.ConstrainedExecution;

namespace OHD_System.Controllers.Admin
{
    public class ExtraActivityController : Controller
    {
        // GET: ExtraActivity
        public ActionResult Index(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            int totalRecords;
            ExtracurricularActivitiesRepository ea =ExtracurricularActivitiesRepository.Instance;
            List<ExtracurricularActivity> ExtraActivity;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                ExtraActivity = ea.SearchActivities(searchTerm, page, pageSize, out totalRecords);
                ViewBag.listExtraActivity = ExtraActivity;
            }
            else
            {
                ExtraActivity = ea.getAllActivity(page, pageSize, out totalRecords);
                ViewBag.listExtraActivity = ExtraActivity;
            }
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SearchTerm = searchTerm;
            return View();
        }
        public ActionResult NewActivity()
        {
            return View();
        }
        [HttpPost]
        public ActionResult createActivity(ExtracurricularActivity model)
        {
            if (model != null)
            {
                if (Request.Form["Status"] == "on")
                {
                    model.Status = 1;
                }
                else
                {
                    model.Status = 0;
                }
                ExtracurricularActivitiesRepository ea=ExtracurricularActivitiesRepository.Instance;
                ea.CreateExtracurricularActivity(model);
                return Redirect("index");
            }
            else
            {
                return Redirect("NewCourse");
            }
        }
        [HttpGet]   
        public ActionResult deleteExtraActivity()
        {
            int idActivity = -1;
            int.TryParse(Request.QueryString["id"], out idActivity);
            if (idActivity == -1)
            {
                return Redirect("Index");
            }
            ExtracurricularActivitiesRepository ea = ExtracurricularActivitiesRepository.Instance;
            ea.DeleteExtracurricularActivity(idActivity);
            return Redirect("index");
        }
        public ActionResult editExtraActivity()
        {
            if (!int.TryParse(Request.QueryString["id"], out int idActivity) || idActivity < 0)
            {
                if (TempData["idActivity"] != null)
                {
                    idActivity = (int)TempData["idActivity"];
                }
                else
                {
                    return Redirect("Index");
                }
            }
            TempData["idActivity"] = idActivity;
            if (TempData["idActivity"] != null)
            {
                ExtracurricularActivitiesRepository ea = ExtracurricularActivitiesRepository.Instance;
                var item = ea.GetActivityByID(idActivity);
                TempData["idActivity"] = idActivity;
                if (TempData["m"] == null)
                {
                    ViewBag.ActivityEdit = item;
                }
                else
                {
                    ViewBag.ActivityEdit = TempData["m"];
                }
                return View();
            }
            return Redirect("index");
        }
        public ActionResult changeExtraActivity(ExtracurricularActivity model)
        {
            if (Request.Form["Status"] == "on")
            {
                model.Status = 1;
            }
            else
            {
                model.Status = 0;
            }
            ExtracurricularActivitiesRepository ea = ExtracurricularActivitiesRepository.Instance;
            model.ActivityID = TempData["idActivity"] != null ? (int)TempData["idActivity"] : 0;
            TempData["m"] = model;
            TempData["idCourse"] = model.ActivityID;
            ea.updateActivity( model.ActivityID,model);
            return Redirect("index");

        }

    }
}