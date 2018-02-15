﻿using SelkiDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SelkiDotNet.Controllers
{
    public class DepartmentsController : ApiController
    {
        LetMeHackEntities db = new LetMeHackEntities();
        // GET: api/Departments
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Departments/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Departments
        public HttpResponseMessage Post([FromBody]DtoDepartment department)
        {
            var check = db.Departments.FirstOrDefault(d => d.Name == department.name);
            if (check != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Department already exits");
            }
            var checkfac = db.Faculties.FirstOrDefault(f => f.Id == department.faculty_id);
            if(checkfac == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "faculty with the given faculty_id does not exist"); 
            }
            DtoDepartmentSuccessMsg1 message = new DtoDepartmentSuccessMsg1();
            DtoDepartmentSuccessMsg2 message1 = new DtoDepartmentSuccessMsg2();
            Department dep = new Department();
            dep.Name = department.name;
            dep.FacultyId = department.faculty_id;
            db.Departments.Add(dep);
            db.SaveChanges();
            var baseUrl1 = Url.Link("DefaultApi", new { controller = "Departments", dep.Id });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
            var baseUrl2 = Url.Link("DefaultApi", new { controller = "Faculties", department.faculty_id });
            message.self = baseUrl1;
            message.name = dep.Name;
            message1.self = baseUrl2.Substring(0, baseUrl2.IndexOf("?")) + "/" + department.faculty_id; ;
            message1.id = department.faculty_id;
            message1.name = db.Faculties.FirstOrDefault(f=>f.Id==department.faculty_id).Name;
            string[] array = new string[3];
            message.faculty = message1;
            var rmsg = Request.CreateResponse(HttpStatusCode.Created, message);
            rmsg.Headers.Location = new Uri(Request.RequestUri + "/" + dep.Id);
            return rmsg;
        }

        // PUT: api/Departments/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Departments/5
        public void Delete(int id)
        {
        }
    }
}
