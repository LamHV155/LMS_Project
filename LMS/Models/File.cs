using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{ 
    public class File
    {
        public File(string fileName, string linkFile)
        {
            this.fileName = fileName;
            this.linkFile = linkFile;
        }
        public string fileName { get; set; }
        public string linkFile { get; set; }
    }
}