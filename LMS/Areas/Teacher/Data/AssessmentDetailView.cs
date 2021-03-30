using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.EF;
namespace LMS.Areas.Teacher.Data
{
    public class AssessmentDetailView
    {
        public COURSE course { get; set; }
        public EVENT eVent{ get; set; }
    }
}