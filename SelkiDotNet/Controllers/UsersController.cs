using SelkiDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

            var check = db.Users.FirstOrDefault(u => u.EmailAddress == user.email);

            if (check != null)
            {
                dto_ConflictMessage message = new dto_ConflictMessage();
                message.message = "An user with email: " + user.email + " already exists.";
                message.status = "409";
                message.developerMessage = "User creation failed because the email: " + user.email + " already exists.";
                return Request.CreateResponse(HttpStatusCode.Conflict, message);
            }
            if (user.email == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Emali Address is empty");
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

                mo.UUID = Guid.NewGuid().ToString();

                if(user.password == null || user.password == "")
                {
                    mo.Password = "";//CreatePassword(8);
                }
                else
                {
                    mo.Password = user.password;
                }
                var verifypassword = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,8}$");
                if (!verifypassword.IsMatch(user.password))
                {
                    dto_ConflictMessage message1 = new dto_ConflictMessage();
                    message1.message = "Password complexity requirement not met.";
                    message1.status = "400";
                    message1.developerMessage = "User creation failed because password complexity requirement not me";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message1);
                }

                mo.Password = MD5Hash(user.password);
                mo.Username = user.username;
                mo.EmailAddress = user.email;
                mo.Role = user.role;
                db.Users.Add(mo);
                db.SaveChanges();

                message.email = user.email;
                var baseUrl = Url.Link("DefaultApi", new { controller = "Users", mo.UUID });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
                message.self = baseUrl.Substring(0, baseUrl.IndexOf("?")) +"/"+ mo.UUID;
                //  rmsg.Headers.Location = new Uri(Request.RequestUri);

                var rmsg = Request.CreateResponse(HttpStatusCode.Created, message);
                rmsg.Headers.Location = new Uri(Request.RequestUri +"/"+ mo.UUID);
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


        //public string CreatePassword(int length)
        //{
        //    const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //    StringBuilder res = new StringBuilder();
        //    Random rnd = new Random();
        //    while (0 < length--)
        //    {
        //        res.Append(valid[rnd.Next(valid.Length)]);
        //    }
        //    return res.ToString();
        //}

        public string CreatePassword(int length)
        {
           return System.Web.Security.Membership.GeneratePassword(length, 1);
        }
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

    }
}
