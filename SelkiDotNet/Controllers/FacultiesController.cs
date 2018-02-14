using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SelkiDotNet.Models;

namespace SelkiDotNet.Controllers
{
    public class FacultiesController : ApiController
    {
        LetMeHackEntities db = new LetMeHackEntities();
        // GET: api/Faculties
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Faculties/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Faculties
        public HttpResponseMessage Post([FromBody]DtoFaculty faculty)
        {
            var check = db.Faculties.FirstOrDefault(u => u.Name == faculty.name);
            if (check != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faculty already exits");
            }
            DtoFacultySuccessMsg message = new DtoFacultySuccessMsg();
            Faculty fac = new Faculty();
            fac.Name = faculty.name;
            db.Faculties.Add(fac);
            db.SaveChanges();
            var baseUrl = Url.Link("DefaultApi", new { controller = "Faculties", fac.Id });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
            message.self = baseUrl;
            message.name = fac.Name;
            var rmsg = Request.CreateResponse(HttpStatusCode.Created, message);
            rmsg.Headers.Location = new Uri(Request.RequestUri + "/" + fac.Id);
            return rmsg;
        }

        // PUT: api/Faculties/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Faculties/5
        public void Delete(int id)
        {
        }
    }
}
