using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models
{
    public class DtoUser
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string uuid { get; set; }
        public string role { get; set; }
        public string mobile { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Nullable<int> faculty_id { get; set; }
        public Nullable<int> department_id { get; set; }
        public Nullable<int> degree_id { get; set; }
        public string batch { get; set; }
    }
}