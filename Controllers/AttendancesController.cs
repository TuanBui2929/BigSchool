using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        public IHttpActionResult Attend(Course attendaceDto)
        {
            var userID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if(context.Attendances.Any(p=>p.Attendee==userID && p.CourseId== attendaceDto.Id))
            {
                return BadRequest("the attendace already exits");
            }
            var attendance = new Attendance() { CourseId = attendaceDto.Id, Attendee = User.Identity.GetUserId() };
            context.Attendances.Add(attendance);
            context.SaveChanges();

            return Ok();
        }

    }
}
