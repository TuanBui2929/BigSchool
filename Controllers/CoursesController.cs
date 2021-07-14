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
            
            ApplicationUser user =System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            course.LecturerId = user.Id;
            bigSchoolContext.Courses.Add(course);
            bigSchoolContext.SaveChanges();


            return RedirectToAction("Index","Home");
        }

        public ActionResult Attending()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
             .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = bigSchoolContext.Attendances.Where(p => p.Attendee == user.Id).ToList();
            var courses = new List<Course>();
            foreach ( Attendance temp in listAttendances)
            {
                Course course = temp.Course;
                course.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                 .FindById(course.LecturerId).Name;
                courses.Add(course);

            }

            return View(courses);
        }

        public ActionResult Mine()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
             .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var courses = bigSchoolContext.Courses.Where(x => x.LecturerId == user.Id && x.DateTime > DateTime.Now).ToList();
            foreach (Course temp in courses)

            {
                temp.Name = user.Name;
               

            }

            return View(courses);
        }
    }
}
