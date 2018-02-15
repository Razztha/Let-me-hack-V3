using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models
{
    public class DtoDepartmentSuccessMsg1
    {
        public string self { get; set; }
        public string name { get; set; }
        public DtoDepartmentSuccessMsg2 faculty { get; set; }
    }
    public class DtoDepartmentSuccessMsg2
    {
        public string self { get; set; }
        public Nullable<int> id { get; set; }
        public string name { get; set; }
    }
}