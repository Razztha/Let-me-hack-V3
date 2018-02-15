using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models
{
    public class DtoDepartment
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> faculty_id { get; set; }
    }
}