using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class RootObject
    {
        public RootObject()
        {
            File = new List<File>();
        }
        public List<File> File { get; set; }
    }
}