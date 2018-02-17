using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SelkiDotNet.Models;
using SelkiDotNet.Models.SuccessMsg;

namespace SelkiDotNet.Controllers
{
    public class FacultiesController : ApiController
    {
        LetMeHackEntities db = new LetMeHackEntities();
        // GET: api/Faculties
        public HttpResponseMessage Get()
        {
            List<Faculty> list = db.Faculties.ToList();
            facultylist fullmessage = new facultylist();
            List<DtoFacultyGetSuccessMsg> fullist = new List<DtoFacultyGetSuccessMsg>();
            foreach (var item in list)
            {
                DtoFacultyGetSuccessMsg message = new DtoFacultyGetSuccessMsg();
                var baseUrl = Url.Link("DefaultApi", new { controller = "Faculties", item.Id });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
                message.self = baseUrl;
                message.id = item.Id;
                message.name = item.Name;
                fullist.Add(message);
            }
            fullmessage.faculties = fullist;
            var rmsg = Request.CreateResponse(HttpStatusCode.Created, fullmessage);
            //rmsg.Headers.Location = new Uri(Request.RequestUri + "/" + fac.Id);
            return rmsg;

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
