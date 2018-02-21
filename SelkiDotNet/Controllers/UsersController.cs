﻿using SelkiDotNet.Models;
using SelkiDotNet.Models.SuccessMsg;
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
        public HttpResponseMessage Get(int faculty=0, int department=0, int degree=0,string q=null)
        {
            
            List<User> list = new List<User>();
            
            if(faculty == 0 && department ==0 && degree==0)
            {
                list = db.Users.ToList();
            }
            if (faculty != 0)
            {
                list = db.Users.Where(d => d.FacultyId == faculty).ToList();             
            }
            if (department !=0)
            {
                list = db.Users.Where(d => d.DepartmentId == department).ToList();
            }
            if (degree !=0)
            {
                list = db.Users.Where(d => d.DegreeId == degree).ToList();
            }
            if(q!=null)
            {
                list = db.Users.Where(d => ((d.Department.Name).Contains(q)) &&
                    (d.Faculty.Name).Contains(q) && (d.Username).Contains(q) && (d.EmailAddress).Contains(q)).ToList();
            }
            
            List<DtoUserGetSuccessMsg> fulllist = new List<DtoUserGetSuccessMsg>();
            userlist ulist = new userlist();
            foreach (var item in list)
            {
                DtoUserGetSuccessMsg message = new DtoUserGetSuccessMsg();
                fact newObjFaculty = new fact();
                dept newObjDepartment = new dept();
                degr newObjDegree = new degr();
                
                var baseUrl = Url.Link("DefaultApi", new { controller = "users", item.UUID });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
                message.self = baseUrl.Substring(0, baseUrl.IndexOf("?")) + "/" + item.UUID; ;
                message.firstname = item.FirstName;
                message.lastname = item.LastName;
                message.batch = item.Batch;
                message.username = item.Username;
                message.email = item.EmailAddress;

                if (item.FacultyId != null)
                {
                    var baseUrl1 = Url.Link("DefaultApi", new { controller = "faculties", item.FacultyId });
                    newObjFaculty.self = baseUrl1.Substring(0, baseUrl1.IndexOf("?")) + "/" + item.FacultyId;
                    newObjFaculty.name = db.Faculties.FirstOrDefault(f => f.Id == item.FacultyId).Name;
                    message.faculty = newObjFaculty;
   
                }
                
                if (item.DepartmentId != null)
                {
                    var baseUrl2 = Url.Link("DefaultApi", new { controller = "departments", item.DepartmentId });
                    newObjDepartment.self = baseUrl2.Substring(0, baseUrl2.IndexOf("?")) + "/" + item.DepartmentId;
                    newObjDepartment.name = db.Departments.FirstOrDefault(f => f.Id == item.DepartmentId).Name;
                    message.department = newObjDepartment;
                }

                if (item.DegreeId != null)
                {
                    var baseUrl3 = Url.Link("DefaultApi", new { controller = "degrees", item.DegreeId });
                    newObjDegree.self = baseUrl3.Substring(0, baseUrl3.IndexOf("?")) + "/" + item.DegreeId;
                    newObjDegree.name = db.Degrees.FirstOrDefault(f => f.Id == item.DegreeId).Name;
                    message.degree = newObjDegree;
                }
                fulllist.Add(message);
            }
            ulist.users = fulllist;
            var rmsg = Request.CreateResponse(HttpStatusCode.Created, ulist);
            return rmsg;
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
                    //user.role = user.role;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid role name");
                }

                mo.UUID = Guid.NewGuid().ToString();

                if(user.password == null || user.password == "")
                {

                    mo.Password = GeneratePassword();

                }
                else
                {
                    mo.Password = user.password;
                }
                var verifypassword = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[~!@#$%^&*_\\-+=`|\\(){}[\\].?]).{6,8}$");
                if (!verifypassword.IsMatch(mo.Password))
                {
                    dto_ConflictMessage message1 = new dto_ConflictMessage();
                    message1.message = "Password complexity requirement not met.";
                    message1.status = "400";
                    message1.developerMessage = "User creation failed because password complexity requirement not me";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message1);
                }

                var verifyphone = new Regex("^[0-9]{11}$");
                if(!(user.mobile == null) || !(user.mobile != ""))
                {
                    if (!verifyphone.IsMatch(user.mobile))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid phone number format");
                    }
                }
               

                fac newObjFaculty = new fac();
                dep newObjDepartment = new dep();
                deg newObjDegree = new deg();
                mo.Password = MD5Hash(mo.Password);
                mo.Username = user.username;
                mo.EmailAddress = user.email;
                mo.Mobile = user.mobile;
                mo.Role = user.role;
                mo.FirstName = user.first_name;
                mo.LastName = user.last_name;
                mo.FacultyId = user.faculty_id;
                mo.DegreeId = user.department_id;
                mo.DegreeId = user.degree_id;
                mo.Batch = user.batch;
                db.Users.Add(mo);
                db.SaveChanges();

                message.email = user.email;
                message.mobile = user.mobile;
                message.first_name = user.first_name;
                message.last_name = user.last_name;
                message.batch = user.batch;

                if (user.faculty_id != null)
                {
                    var baseUrl1 = Url.Link("DefaultApi", new { controller = "faculties", user.faculty_id });
                    newObjFaculty.self = baseUrl1.Substring(0, baseUrl1.IndexOf("?")) + "/" + user.faculty_id;
                    newObjFaculty.name = db.Faculties.FirstOrDefault(f => f.Id == user.faculty_id).Name;
                    message.faculty = newObjFaculty;
                }
                
                if(user.department_id != null)
                {
                    var baseUrl2 = Url.Link("DefaultApi", new { controller = "departments", user.department_id });
                    newObjDepartment.self = baseUrl2.Substring(0, baseUrl2.IndexOf("?")) + "/" + user.department_id;
                    newObjDepartment.name = db.Departments.FirstOrDefault(f => f.Id == user.department_id).Name;
                    message.department = newObjDepartment;
                }

                if (user.degree_id!= null)
                {
                    var baseUrl3 = Url.Link("DefaultApi", new { controller = "degrees", user.degree_id });
                    newObjDegree.self = baseUrl3.Substring(0, baseUrl3.IndexOf("?")) + "/" + user.degree_id;
                    newObjDegree.name = db.Degrees.FirstOrDefault(f => f.Id == user.degree_id).Name;
                    message.degree = newObjDegree;
                }
                

                var baseUrl = Url.Link("DefaultApi", new { controller = "users", mo.UUID });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/
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
        
        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        private String GeneratePassword(int genlen = 8, bool usenumbers = true, bool uselowalphabets = true, bool usehighalphabets = true, bool usesymbols = true)
        {

            var upperCase = new char[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z'
            };

            var lowerCase = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z'
            };

            var numerals = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            var symbols = new char[]
            {
                '@', '#', '$', '%', '^', '&', '*','{', '[', '}', ']'
                //'~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '{', '[', '}', ']', '-', '_', '=', '+', ':',
                //';', '|', '/', '?', ',', '<', '.', '>'
            };

            char[] total = (new char[0])
                            .Concat(usehighalphabets ? upperCase : new char[0])
                            .Concat(uselowalphabets ? lowerCase : new char[0])
                            .Concat(usenumbers ? numerals : new char[0])
                            .Concat(usesymbols ? symbols : new char[0])
                            .ToArray();

            var rnd = new Random();

            var chars = Enumerable
                .Repeat<int>(0, genlen)
                .Select(i => total[rnd.Next(total.Length)])
                .ToArray();

            return new string(chars);
        }

    }
}
