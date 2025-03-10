using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OHD_System.Models.Entities;
using OHD_System.Models.ModelView;
using OHD_System.Models.Repositories;

namespace OHD_System.Controllers.Admin
{
    public class UserController : Controller
    {
        // GET: User
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public ActionResult Index(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            int totalRecords;
            UserRepository us = UserRepository.Instance;
            List<UserView> users ;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = us.SearchProducts(searchTerm, page, pageSize, out totalRecords);
                ViewBag.ListUser = users;
            }
            else
            {
                users = us.getAllProduct(page, pageSize, out totalRecords);
                ViewBag.ListUser = users;
            }
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SearchTerm = searchTerm;
            return View();
        }
        public ActionResult NewUser()
        {
            RoleRepository role = RoleRepository.Instance;
            ViewBag.listRole = role.getAllRole();
            return View();
        }
        [HttpPost]
        public ActionResult createUser(User model)
        {
            if (Request.Form["Status"] == "on")
            {
                model.Status = 1;
            }
            else
            {
                model.Status = 0;
            }
            model.PasswordHash = HashPassword(model.PasswordHash);
            UserRepository us = UserRepository.Instance;
            us.CreateUser(model);
            return Redirect("index");
        }
    }
}