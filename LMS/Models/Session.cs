using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Session
    {
        public string id_user { get; set; }
        public string id_semester { get; set; }
        public string id_course { get; set; }
        public string role { get; set; }
    }
}