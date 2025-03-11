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
            List<UserView> users;
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
        public ActionResult EditUser()
        {
            if (!int.TryParse(Request.QueryString["id"], out int idUser) || idUser < 0)
            {
                if (TempData["idUser"] != null)
                {
                    idUser = (int)TempData["idUser"];
                }
                else
                {
                    return Redirect("Index");
                }
            }
            TempData["idUser"] = idUser;
            if (TempData["idUser"] != null)
            {
                RoleRepository role = RoleRepository.Instance;
                ViewBag.listRole = role.getAllRole();
                UserRepository us = UserRepository.Instance;
                var item = us.getUserByID(idUser);
                TempData["idUser"] = idUser;
                if (TempData["m"] == null)
                {
                    ViewBag.UserEdit = item;
                }
                else
                {
                    ViewBag.UserEdit = TempData["m"];
                }
                return View();
            }
            return Redirect("index");
        }
        public ActionResult changeUser(User model)
        {
            if (Request.Form["Status"] == "on")
            {
                model.Status = 1;
            }
            else
            {
                model.Status = 0;
            }
            UserRepository us = UserRepository.Instance;
            model.UserID = TempData["idUser"] != null ? (int)TempData["idUser"] : 0;
            TempData["m"] = model;
            TempData["idUser"] = model.UserID;
            us.updateUser(model.UserID, model);
            return Redirect("index");
        }
        public ActionResult deleteUser()
        {
            int idUser = -1;
            int.TryParse(Request.QueryString["id"], out idUser);
            if (idUser == -1)
            {
                return Redirect("Index");
            }
            UserRepository us = UserRepository.Instance;
            us.DeleteUser(idUser);
            return Redirect("Index");
        }
    }
}