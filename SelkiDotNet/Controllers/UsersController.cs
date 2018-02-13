using SelkiDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SelkiDotNet.Controllers
{
    public class UsersController : ApiController
    {
        LetMeHackEntities db = new LetMeHackEntities();

        // GET: api/Users
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Users/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Users
        public HttpResponseMessage Post([FromBody]DtoUser user)
        {

            var check = db.Users.FirstOrDefault(u => u.EmailAddress == user.emailaddress);

            if (check != null)
            {
                dto_ConflictMessage message = new dto_ConflictMessage();
                message.message =  "An user with email: " + user.emailaddress + " already exists.";
                message.status = "409";
                message.developerMessage = "User creation failed because the email: " + user.emailaddress + " already exists.";
                return Request.CreateResponse(HttpStatusCode.Conflict, message);
            }
            if (user.emailaddress == null || user.password == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Emali Address or Password is empty");
            }
            else
            {
                dto_Successful_Message message = new dto_Successful_Message();
                User mo = new User();
                if(user.role == null || user.role == "")
                {
                    message.role = "user";
                    user.role = "user";
                }
                else if (user.role == "user" || user.role == "admin" || user.role == "moderator")
                {
                    message.role = user.role;
                    user.role = user.role;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid role name");
                }
                
                mo.EmailAddress = user.emailaddress;
                mo.Password = user.password;
                mo.Role = user.role;
                db.Users.Add(mo);
                db.SaveChanges();
                
                message.emailaddress = user.emailaddress;
                var baseUrl = Url.Link("DefaultApi", new { controller = "Users", mo.ID });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
                message.self = baseUrl.ToString();
                //  rmsg.Headers.Location = new Uri(Request.RequestUri);

                var rmsg = Request.CreateResponse(HttpStatusCode.Created, message);
                rmsg.Headers.Location = new Uri(Request.RequestUri +"/"+ mo.ID.ToString());
                return rmsg;
            }

        }

        // PUT: api/Users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Users/5
        public void Delete(int id)
        {
        }
    }
}
