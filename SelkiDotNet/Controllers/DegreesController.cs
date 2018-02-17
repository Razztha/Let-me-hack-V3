using SelkiDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SelkiDotNet.Controllers
{
    public class DegreesController : ApiController
    {
        LetMeHackEntities db = new LetMeHackEntities();
        // GET: api/Degrees
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Degrees/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Degrees
        public HttpResponseMessage Post([FromBody]DtoDegrees degree)
        {
            var check = db.Degrees.FirstOrDefault(d => d.Name == degree.name);
            if (check != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Degree already exits");
            }
            var checkfac = db.Departments.FirstOrDefault(f => f.Id == degree.department_id);
            if (checkfac == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "department with the given department_id does not exist");
            }
            DtoDepartmentSuccessMsg1 message = new DtoDepartmentSuccessMsg1();
            DtoDepartmentSuccessMsg2 message1 = new DtoDepartmentSuccessMsg2();
            Degree deg = new Degree();
            deg.Name = degree.name;
            deg.DepartmentId = degree.department_id;
            db.Degrees.Add(deg);
            db.SaveChanges();
            var baseUrl1 = Url.Link("DefaultApi", new { controller = "degrees", deg.Id });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
            var baseUrl2 = Url.Link("DefaultApi", new { controller = "departments", degree.department_id });
            message.self = baseUrl1;
            message.name = deg.Name;
            message1.self = baseUrl2.Substring(0, baseUrl2.IndexOf("?")) + "/" + degree.department_id; ;
            message1.id = degree.department_id;
            message1.name = db.Faculties.FirstOrDefault(f => f.Id == degree.department_id).Name;
            message.faculty = message1;
            var rmsg = Request.CreateResponse(HttpStatusCode.Created, message);
            rmsg.Headers.Location = new Uri(Request.RequestUri + "/" + deg.Id);
            return rmsg;
        }


        // PUT: api/Degrees/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Degrees/5
        public void Delete(int id)
        {
        }
    }
}
