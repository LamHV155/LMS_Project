using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.DAO;
using DAL.StudentView;
using DAL.EF;
using LMS.Common;

namespace LMS.Areas.Student.Controllers
{
    [CustomAuthorize("STUDENT")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CommonFunc cFunc = new CommonFunc();
            SubmitDAO dao = new SubmitDAO();
            List<COURSE> courses = dao.GetDeadlinebyStuAndCourseAndSem(cFunc.GetIdUserBySession());
            List<DeadlineView> dView = new List<DeadlineView>();

            foreach (COURSE course in courses)
            {
                if (course.TOPICs == null) continue;
                foreach (TOPIC topic in course.TOPICs)
                {
                    if (topic.EVENTs == null) continue;
                    foreach (EVENT ev in topic.EVENTs)
                    {
                        DeadlineView dV = new DeadlineView()
                        {
                            courseID = course.ID,
                            courseName = course.NAME,
                            eventID = ev.ID,
                            eventTitle = ev.TITLE,
                            eventDeadline = ev.DEADLINE
                        };
                        if (ev.SUBMITs == null)
                        {
                            dView.Add(dV);
                            continue;
                        }
                        foreach (SUBMIT submit in ev.SUBMITs)
                        {
                            dV.submitID = submit.ID;
                            dView.Add(dV);
                        }
                    }
                }
            }
            var ListEvent = from s in dView select s;
            ListEvent = ListEvent.Where(s => s.eventDeadline > DateTime.Now);
            ListEvent = ListEvent.OrderBy(s => s.eventDeadline);
            return View(ListEvent.ToList());
        }
    }
}