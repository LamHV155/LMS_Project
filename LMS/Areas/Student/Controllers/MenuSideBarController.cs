using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.DAO;
using DAL.EF;
using LMS.Common;
using LMS.Models;


namespace LMS.Areas.Student.Controllers
{
    public class MenuSideBarController : Controller
    {
        //[CustomAuthorize("STUDENT")]
        //GET: Student/MenuSideBar
        [ChildActionOnly]
        public ActionResult Index()
        {
            CommonFunc cf = new CommonFunc();

            SemesterDAO semDao = new SemesterDAO();
            SEMESTER sem = semDao.GetSemesterByID(cf.GetIdSemesterBySession());
            UserDAO userDao = new UserDAO();
            Session["UserName"] = userDao.GetUserByID(cf.GetIdUserBySession()).FIRST_NAME;

            return View(sem);
        }


        [ChildActionOnly]
        public ActionResult Semester()
        {
            SemesterDAO semDAO = new SemesterDAO();
            var listSem = semDAO.GetSEMESTERs();

            return View(listSem);
        }

        [ChildActionOnly]
        public ActionResult Subjects()
        {
            CommonFunc cFunc = new CommonFunc();
            CourseDAO courseDAO = new CourseDAO();
            var listCourse = courseDAO.GetCourseByStudentAndSemester(cFunc.GetIdUserBySession(), cFunc.GetIdSemesterBySession());
            return View(listCourse);
        }


        [ChildActionOnly]
        public ActionResult SubjectAssessments()
        {
            CommonFunc cFunc = new CommonFunc();
            CourseDAO courseDao = new CourseDAO();
            var listCourse = courseDao.GetCourseByStudentAndSemester(cFunc.GetIdUserBySession(), cFunc.GetIdSemesterBySession());
            return View(listCourse);
        }
    }

}