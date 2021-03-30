using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace DAL.StudentView
{
    public class CourseDetailsView
    {
        public CourseDetailsView()
        {
            topics = new List<TOPIC>();
        }
        public List<TOPIC> topics { get; set; }
        public COURSE course;
    }
}
