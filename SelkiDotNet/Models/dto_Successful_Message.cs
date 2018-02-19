using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models
{
    public class dto_Successful_Message
    {
        public string self { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string mobile { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string batch { get; set; }
        public string username { get; set; }
        public fac faculty { get; set; }
        public dep department { get; set; }
        public deg degree { get; set; }

    }
    public class fac
    {
        public string self { get; set; }
        public string name { get; set; }
    }
    public class dep
    {
        public string self { get; set; }
        public string name { get; set; }
    }
    public class deg
    {
        public string self { get; set; }
        public string name { get; set; }
    }
}