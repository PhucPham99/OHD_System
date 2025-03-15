using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.Mvc;
using OHD_System.Models.Entities;
using OHD_System.Models.ModelView;
using OHD_System.Models.Repositories;
using OHD_System.Models.Utils;

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
            ClearSessionRegex();    
            int totalRecords;
            UserRepository us = UserRepository.Instance;
            List<UserView> users;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = us.SearchUser(searchTerm, page, pageSize, out totalRecords);
                ViewBag.ListUser = users;
            }
            else
            {
                users = us.getAllUser(page, pageSize, out totalRecords);
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
        public ActionResult createUser(User model, string ConfirmPassword)
        {
            if (model != null)
            {
                model.Status = Request.Form["Status"] == "on" ? 1 : 0;
                Session["m"] = model;
                bool check = false;
                UserRepository us = UserRepository.Instance;
                MyValidate val = MyValidate.Instance;
                if (us.checkUser(model.FullName).Count != 0)
                {
                    Session["RegexUserName"] = "ClassName was exist";
                }
                else
                {
                    Session["RegexUserName"] = val.CheckIDUser(model.FullName, "CourseName");
                }
                if (us.checkUser(model.PhoneNumber).Count != 0)
                {
                    Session["RegexPhone"] = "Phone was exist";
                }
                else
                {
                    Session["RegexPhone"] = val.CheckPhoneNumber(model.PhoneNumber);
                }
                if (us.checkUser(model.Email).Count != 0)
                {
                    Session["RegexEmail"] = "Email was exist";
                }
                else
                {
                    Session["RegexEmail"] = val.CheckEmail(model.Email);
                }
                if (model.PasswordHash != ConfirmPassword)
                {
                    Session["RegexConfirmPassword"] = "Confirm Password does not match!";
                }
                else
                {
                    Session["RegexConfirmPassword"] = "";
                }
                Session["RegexRole"] = val.CheckType(model.RoleID.ToString(), "Role");
                Session["RegexPassword"] = val.CheckPassword(model.PasswordHash);
                Session["RegexCreatedAt"] = val.ValidateCreatedAt(model.CreatedAt.ToString(), "CreatedAt");
                Session["RegexUpdateAt"] = val.ValidateUpdatedAt(model.UpdatedAt.ToString(), "updateAt");
                if (!Session["RegexUserName"].Equals("") || !Session["RegexPhone"].Equals("") || !Session["RegexEmail"].Equals("") || !Session["RegexRole"].Equals("") || !Session["RegexCreatedAt"].Equals("") || !Session["RegexUpdateAt"].Equals("") || !Session["RegexConfirmPassword"].Equals(""))
                {
                    check = true;
                }
                if (check == true)
                {
                    return Redirect("NewUser");
                }
                else
                {
                    model.PasswordHash = HashPassword(model.PasswordHash);
                    us.CreateUser(model);
                    return Redirect("index");
                }
            }
            return Redirect("newuser");
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
        [HttpGet]
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
        private void ClearSessionRegex()
        {
            string[] sessionKeys = {
        "RegexUserName",
        "RegexPhone",
        "RegexEmail",
        "RegexRole",
        "RegexCreatedAt",
        "RegexUpdateAt"
    };

            foreach (string key in sessionKeys)
            {
                Session.Remove(key);
            }
        }

    }
}