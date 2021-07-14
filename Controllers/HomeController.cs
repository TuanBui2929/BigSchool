using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
  
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            BigSchoolContext bigSchoolContext = new BigSchoolContext();
            var upcomming = bigSchoolContext.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(m => m.DateTime).ToList();
            foreach (Course i in upcomming)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;
            }
            return View(upcomming);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}