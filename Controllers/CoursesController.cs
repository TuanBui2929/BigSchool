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
    public class CoursesController : Controller
    {
        BigSchoolContext bigSchoolContext = new BigSchoolContext();

        public ActionResult Index()
        {
            return View();
        }
        // GET: Courses
        public ActionResult Create()
        {
            Course course = new Course();
            course.Listcategory = bigSchoolContext.Categories.ToList();


            return View(course);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                course.Listcategory = bigSchoolContext.Categories.ToList();
                return View("Create", course);

            }
            
            ApplicationUser user =System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            course.LecturerId = user.Id;
            bigSchoolContext.Courses.Add(course);
            bigSchoolContext.SaveChanges();


            return RedirectToAction("Index","Home");
        }
    }
}
