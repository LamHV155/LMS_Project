using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Common;
using DAL.DAO;
using DAL.EF;

namespace LMS.Areas.Admin.Controllers
{
    [CustomAuthorize("ADMIN")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            CommonFunc cFunc = new CommonFunc();
            return View();
        }
        public JsonResult GetPieData ()
        {
            int male = new InfoTeacherDAO().GetMaleTeacher();
            int female = new InfoTeacherDAO().GetFemaleTeacher();
            int total = male + female;
            float MalePercent = (float)male / total;
            float FemalePercent = (float)female / total;
            return Json(new
            {
                male = Math.Round(MalePercent,2),
                female= Math.Round(FemalePercent, 2)
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CountTeacher()
        {
            var cntTeacher = new InfoTeacherDAO().CountTeacher();
            return Json(new
            {
                data = cntTeacher
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CountStudent()
        {
            var cntStudent = new InfoStudentDAO().CountStudent();
            return Json(new
            {
                data = cntStudent
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CountCourse()
        {
            var cntCourse = new CourseDAO().CountCourse();
            return Json(new
            {
                data = cntCourse
            },JsonRequestBehavior.AllowGet);
        }
        public JsonResult CountSubjects()
        {
            var cntSubjects = new SubjectsDAO().countsubjects();
            return Json(new
            {
                data = cntSubjects
            }, JsonRequestBehavior.AllowGet);
        }
    }
}