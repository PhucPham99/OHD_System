using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OHD_System.Models.Repositories;

namespace OHD_System.Controllers.Admin
{
    public class ExtracurricularRegistrationController : Controller
    {
        // GET: ExtracurricularRegistration
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewExtraRegistration()
        {
            ExtracurricularActivitiesRepository act = ExtracurricularActivitiesRepository.Instance;
            UserRepository userRepository = UserRepository.Instance;
            ViewBag.listActivit=act.GetActivities();
            ViewBag.listUser = userRepository.GetUsers();
            return View();
        }
    }
}