using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelkiDotNet.Models.SuccessMsg
{
    public class DtoUserGetSuccessMsg
    {
        public string self { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string batch { get; set; }
        public fact faculty { get; set; }
        public dept department { get; set; }
        public degr degree { get; set; }

    }
    public class fact
    {
        public string self { get; set; }
        public string name { get; set; }
    }
    public class dept
    {
        public string self { get; set; }
        public string name { get; set; }
    }
    public class degr
    {
        public string self { get; set; }
        public string name { get; set; }
    }
}