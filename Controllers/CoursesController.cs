using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
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

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course x = bigSchoolContext.Courses.FirstOrDefault(m => m.Id == id);
            if (x == null)
            {
                return HttpNotFound();
            }
            return View(x);
        }
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMine(int id)
        {         
            Course x = bigSchoolContext.Courses.FirstOrDefault(m => m.Id == id);
            var y = bigSchoolContext.Attendances.Where(m => m.CourseId == x.Id).ToList();
            if (x != null)
                {
                    foreach( Attendance att in y)
                    {
                         bigSchoolContext.Attendances.Remove(att);
                    }
       
                    bigSchoolContext.Courses.Remove(x);
                    bigSchoolContext.SaveChanges();
                    return RedirectToAction("Mine");
                    
                }
 

            return View();
        }

        public ActionResult Edit(int id)
        {
           
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course x = bigSchoolContext.Courses.FirstOrDefault(m => m.Id == id);
            x.Listcategory = bigSchoolContext.Categories.ToList();
            if (x == null)
            {
                return HttpNotFound();
            }
            return View(x);
        }
        [Authorize]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditMine(Course course)
        {

           // ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                course.Listcategory = bigSchoolContext.Categories.ToList();
                return View("Edit", course);

            }

            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            //    .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //course.LecturerId = user.Id;
            bigSchoolContext.Courses.AddOrUpdate(course);
            bigSchoolContext.SaveChanges();

            return RedirectToAction("Index","Home");
        }


    }
}
