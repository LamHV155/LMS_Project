﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DAL.DAO;
using DAL.EF;
using Newtonsoft.Json;

namespace LMS.Areas.Admin.Controllers
{
    public class TeacherController : Controller
    {
        LMSProjectDBContext db = new LMSProjectDBContext();
        // GET: Admin/Teacher
        public ActionResult Index()
        {
            return View();
        }
        //Done
        public JsonResult GetTeacher(int page, int pageSize)
        {
            var model = new InfoTeacherDAO().getteacher().Select(x => new
            {
                ID = x.ID,
                FIRST_NAME = x.FIRST_NAME,
                LAST_NAME = x.LAST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                PHONE_NO = x.PHONE_NO,
                DoB = x.DoB,
                MAIL = x.MAIL,
                FACULTY = new
                {
                    ID = x.FACULTY.ID,
                    NAME = x.FACULTY.NAME
                }
            });
            var subjects = model.Skip((page - 1) * pageSize).Take(pageSize);
            int totalRow = model.Count();
            return Json(new
            {
                total = totalRow,
                data = subjects,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        //Done
        public JsonResult Delete(string id)
        {
            var model = new InfoTeacherDAO().deleteteacher(id);
            return Json(new
            {
                status = true
            });
        }
        //Done
        public JsonResult Detail(string idnameteacher)
        {
            var model = new InfoTeacherDAO().detailteacher(idnameteacher).Select(x => new
            {
                ID = x.ID,
                FIRST_NAME = x.FIRST_NAME,
                LAST_NAME = x.LAST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                PHONE_NO = x.PHONE_NO,
                DoB = x.DoB,
                SEX = x.SEX,
                MAIL = x.MAIL,
                PASSWORD = x.PASSWORD,
                LASTVISITDATE = x.LASTVISITDATE,
                FACULTY = new { ID = x.FACULTY.ID, NAME = x.FACULTY.NAME }
            }).ToList();
            return Json(new
            {
                data = model,
                status = true
            });
        }

        public JsonResult Save(C_USER infoteacher)
        {
            bool status = false;
            string message = string.Empty;
            if (infoteacher.ID != null)
            {
                try
                {
                    var model = new InfoTeacherDAO().updateteacher(infoteacher);
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }
        public JsonResult GetFacultyID_NAME()
        {
            var model = new FacultyDAO().getfaculty().Select(x => new { ID = x.ID, NAME = x.NAME });
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCoursebyID(string idteacher = "U00003")
        {
            var sub = new InfoTeacherDAO().getcoursebyID(idteacher).Select(x => new
            {
                ID = x.ID,
                FIRST_NAME = x.FIRST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                LAST_NAME = x.LAST_NAME,
                TEACHES = x.TEACHES.Select(b => new
                {
                    COURSE = new COURSE
                    {
                        ID = b.COURSE.ID,
                        NAME = b.COURSE.NAME,
                        SEMESTER = new SEMESTER
                        {
                            ID = b.COURSE.SEMESTER.ID,
                            TITLE = b.COURSE.SEMESTER.TITLE,
                        }
                    }
                })
            });
            return Json(new
            {
                data = sub,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCoursebyID(string idcourse)
        {
            var model = new CourseDAO().deletecourse(idcourse);
            return Json(new
            {
                status = true
            });
        }
    }
}