using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OHD_System.Models.Entities;
using OHD_System.Models.Repositories;
using OHD_System.Models.Utils;

namespace OHD_System.Controllers.Admin
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            ClearSessionRegex();
            int totalRecords;
            CourseRepository cor = CourseRepository.Instance;
            List<Cours> courses;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                courses = cor.SearchCourses(searchTerm, page, pageSize, out totalRecords);
                ViewBag.listCourse = courses;
            }
            else
            {
                courses = cor.getAllCourse(page, pageSize, out totalRecords);
                ViewBag.listCourse = courses;
            }
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SearchTerm = searchTerm;
            return View();
        }
        public ActionResult NewCourse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCourse(Cours model)
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
                Session["m"] = model;
                CourseRepository cor = CourseRepository.Instance;
                bool check = false;
                MyValidate val = MyValidate.Instance;
                if (cor.checkCourseName(model.CourseName).Count != 0)
                {
                    Session["RegexCourseName"] = "ClassName was exist";
                }
                else
                {
                    Session["RegexCourseName"] = val.CheckCourseName(model.CourseName, "CourseName");
                }
                Session["RegexCredits"] = val.CheckNumbers(model.Credits.ToString(), "Credits");
                Session["RegexDeparment"] = val.CheckDepartment(model.Department, "Department");
                Session["RegexParticipants"] = val.CheckNumbers(model.participants.ToString(), "Participants");
                Session["RegexDescription"] = val.CheckInput(model.Description, "Description");
                Session["RegexCreatedAt"] = val.ValidateCreatedAt(model.CreatedAt.ToString(), "CreatedAt");
                Session["RegexEndDateAt"] = val.CheckEndDateAt(model.endDateAt.ToString(), model.CreatedAt.ToString());
                if (!Session["RegexCourseName"].Equals("") || !Session["RegexCredits"].Equals("") || !Session["RegexDeparment"].Equals("") || !Session["RegexParticipants"].Equals("") || !Session["RegexDescription"].Equals("") || !Session["RegexCreatedAt"].Equals("") || !Session["RegexEndDateAt"].Equals(""))
                {
                    check = true;
                }
                if (check == true)
                {
                    return Redirect("NewCourse");
                }
                else
                {
                    cor.addCourse(model);
                    return Redirect("index");

                }
            }
            else
            {
                return Redirect("NewCourse");
            }
        }
        [HttpGet]
        public ActionResult deleteCourse()
        {
            int idCourse = -1;
            int.TryParse(Request.QueryString["id"], out idCourse);
            if (idCourse == -1)
            {
                return Redirect("Index");
            }
            CourseRepository cor = CourseRepository.Instance;
            cor.deleteCourse(idCourse);
            return Redirect("index");
        }
        public ActionResult EditCourse()
        {
            if (!int.TryParse(Request.QueryString["id"], out int idCourse) || idCourse < 0)
            {
                if (TempData["idCourse"] != null)
                {
                    idCourse = (int)TempData["idCourse"];
                }
                else
                {
                    return Redirect("Index");
                }
            }
            TempData["idCourse"] = idCourse;
            if (TempData["idCourse"] != null)
            {
                RoleRepository role = RoleRepository.Instance;
                CourseRepository courseRepository = CourseRepository.Instance;
                var item = courseRepository.getCourseByID(idCourse);
                TempData["idCourse"] = idCourse;
                if (TempData["m"] == null)
                {
                    ViewBag.CourseEdit = item;
                }
                else
                {
                    ViewBag.CourseEdit = TempData["m"];
                }
                return View();
            }
            return Redirect("index");
        }
        public ActionResult changeCourse(Cours model)
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
                Session["m"] = model;
                CourseRepository cor = CourseRepository.Instance;
                bool check = false;
                MyValidate val = MyValidate.Instance;
                model.CourseID = TempData["idCourse"] != null ? (int)TempData["idCourse"] : 0;
                if (cor.checkCourseName(model.CourseName).Any(e=>e.CourseID!=model.CourseID))
                {
                    Session["RegexCourseName"] = "ClassName was exist";
                }
                else
                {
                    Session["RegexCourseName"] = val.CheckCourseName(model.CourseName, "CourseName");
                }
                Session["RegexCredits"] = val.CheckNumbers(model.Credits.ToString(), "Credits");
                Session["RegexDeparment"] = val.CheckDepartment(model.Department, "Department");
                Session["RegexParticipants"] = val.CheckNumbers(model.participants.ToString(), "Participants");
                Session["RegexDescription"] = val.CheckInput(model.Description, "Description");
                Session["RegexCreatedAt"] = val.ValidateCreatedAt(model.CreatedAt.ToString(), "CreatedAt");
                Session["RegexEndDateAt"] = val.CheckEndDateAt(model.endDateAt.ToString(), model.CreatedAt.ToString());
                if (!Session["RegexCourseName"].Equals("") || !Session["RegexCredits"].Equals("") || !Session["RegexDeparment"].Equals("") || !Session["RegexParticipants"].Equals("") || !Session["RegexDescription"].Equals("") || !Session["RegexCreatedAt"].Equals("") || !Session["RegexEndDateAt"].Equals(""))
                {
                    check = true;
                }
                if (check == true)
                {
                    return Redirect("NewCourse");
                }
                else
                {
                    TempData["m"] = model;
                    TempData["idCourse"] = model.CourseID;
                    cor.updateCourse(model, model.CourseID);
                    return Redirect("index");
                }
            }
            return Redirect("editcourse");
        }
        private void ClearSessionRegex()
        {
            Session.Remove("RegexCourseName");
            Session.Remove("RegexCredits");
            Session.Remove("RegexDeparment");
            Session.Remove("RegexParticipants");
            Session.Remove("RegexCreatedAt");
            Session.Remove("RegexEndDateAt");
            Session.Remove("m");
        }
    }
}