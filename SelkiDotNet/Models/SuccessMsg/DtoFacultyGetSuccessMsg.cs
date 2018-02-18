using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models.SuccessMsg
{
    
        public class DtoFacultyGetSuccessMsg
        {
            public string self { get; set; }
            public string name { get; set; }
            public int id { get; set; }
            
        }
        public class facultylist
        {
            public List<DtoFacultyGetSuccessMsg> faculties { get; set; }
        }
}