using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OHD_System.Models.Entities;
using OHD_System.Models.Repositories;

namespace OHD_System.Controllers.Admin
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            int totalRecords;
            CourseRepository cor = CourseRepository.Instance;
            List<Cours> courses;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                courses = cor.SearchCourses(searchTerm, page, pageSize, out totalRecords);
                ViewBag.ListUser = courses;
            }
            else
            {
                courses = cor.getAllCourse(page, pageSize, out totalRecords);
                ViewBag.ListUser = courses;
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
        public ActionResult CreateCourse()
        {
            return Redirect("index");
        }
    }
}