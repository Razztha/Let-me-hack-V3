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
    }
}