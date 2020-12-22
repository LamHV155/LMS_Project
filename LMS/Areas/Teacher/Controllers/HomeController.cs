﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.DAO;
using DAL.EF;
using LMS.Common;
using LMS.Models;
namespace LMS.Areas.Teacher.Controllers
{
    public class HomeController : Controller
    {
        // GET: Teacher/Home
        public ActionResult Index(string semester_id)
        {
            
            CommonFunc cFunc = new CommonFunc();
            if(cFunc.CheckSectionInvalid() == false || cFunc.checkRole("TEACHER") == false) 
                return RedirectToAction("Error", "Home", new { area = "" });
           

            if (semester_id == null)
            {
                semester_id = cFunc.GetIdSemesterBySession();
            }
            CourseDAO courseDao = new CourseDAO();
            List<COURSE> listCourse = courseDao.GetCourseByTeacherAndSemester(cFunc.GetIdUserBySession(), cFunc.GetIdSemesterBySession());       
                return View(listCourse);
        }
    }
}