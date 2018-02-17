using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models.SuccessMsg
{
    public class DtoDepartmentGetSuccessMsg
    {
            public string self { get; set; }
            public int id { get; set; }
            public string name { get; set; }

    }
        public class DepartmentList
        {
            public List<DtoDepartmentGetSuccessMsg> departments { get; set; }
        }
    
}