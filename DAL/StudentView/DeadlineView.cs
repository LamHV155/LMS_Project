using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.StudentView
{
    public class DeadlineView
    {
        public string courseID { set; get; }
        public string courseName { set; get; }
        public string eventID { set; get; }
        public string eventTitle { set; get; }
        public DateTime? eventDeadline { set; get; }
        public string submitID { get; set; }
    }
}
