using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.StudentView
{
    public class SubmitAssessmentView
    {
        public string courseID { get; set; }
        public string courseName { get; set; }
        public string topicID { get; set; }
        public string topicTitle { get; set; }
        public string eventID { get; set; }
        public string eventTitle { get; set; }
        public string eventDescription { get; set; }
        public DateTime? eventDeadline { get; set; }
        public string submitID { get; set; }
        public string submitLink { get; set; }
        public DateTime? submitTime { get; set; }
        public float assScore { get; set; }
        public string assComment { get; set; }
    }
}
