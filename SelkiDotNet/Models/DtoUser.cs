using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models
{
    public class DtoUser
    {
        public string emailaddress { get; set; }
        public string password { get; set; }
        public int id { get; set; }
        public string role { get; set; }
    }
}