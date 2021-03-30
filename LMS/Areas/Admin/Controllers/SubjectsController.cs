using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DAL.DAO;
using DAL.EF;
using LMS.Common;

namespace LMS.Areas.Admin.Controllers
{
    [CustomAuthorize("ADMIN")]
    public class SubjectsController : Controller
    {
        LMSProjectDBContext db = new LMSProjectDBContext();
        // GET: Admin/Subjects
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetSubjects(string name,int page, int pageSize)
        {
            var ListSubjects = new SubjectsDAO().getsubject().Select(a => new
            {
                ID = a.ID,
                NAME = a.NAME,
                DESCRIPTION = a.DESCRIPTION
            });
            if (!string.IsNullOrEmpty(name))
            {
                ListSubjects = ListSubjects.Where(x => x.NAME.Contains(name));
            }
            int TotalRow = ListSubjects.Count();
            var lstSubjects = ListSubjects.Skip((page - 1) * pageSize).Take(pageSize);   
            return Json(new
            {
                total = TotalRow,
                data = lstSubjects,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string id)
        {
            var model = new SubjectsDAO().delete(id);
            var sub = new SubjectsDAO().getsubject().Select(a => new
            {
                ID = a.ID,
                NAME = a.NAME,
                DESCRIPTION = a.DESCRIPTION
            });
            int totalRow = sub.Count();
            return Json(new
            {
                total = totalRow,
                status = true
            });
        }
        public JsonResult Detail(string id)
        {
            var model = new SubjectsDAO().getdetail(id).Select(x => new {
                ID = x.ID,
                NAME = x.NAME,
                DESCRIPTION = x.DESCRIPTION,
                FACULTY = new FACULTY
                {
                    ID = x.FACULTY.ID,
                    NAME = x.FACULTY.NAME,
                }
            });
            return Json(new
            {
                data = model,
                status = true
            },JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save(SUBJECT subjects)
        {
            bool status = false;
            string message = string.Empty;
            int sub = new SubjectsDAO().CheckSubjects(subjects.NAME);
            if (subjects.ID != "0")
            {
                try
                {
                    var model = new SubjectsDAO().update(subjects);
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }
            else
            {
                try
                {
                    if (sub==0)
                    {
                        subjects.ID = createID("SUBJ");
                        db.SUBJECTs.Add(subjects);
                        db.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }
        public string createID(string code)
        {
            var builder = new StringBuilder();
            builder.Append(code + DateTime.Now.ToString("yyyyMMddHHmmssff"));
            return builder.ToString();
        }
        public JsonResult GetCourseInSubject(string idsub)
        {
            var model = new SubjectsDAO().getcourseinsubjects(idsub).Select(a => new
            {
                ID = a.ID,
                NAME = a.NAME,
                COURSE = a.COURSEs.Select(b => new
                {
                    IDCOURSE = b.ID,
                    NAMECOURSE = b.NAME,
                    DESCRIPTION = b.DESCRIPTION,
                    TEACH = new TEACH
                    {
                        C_USER = new C_USER
                        {
                            ID = b.TEACH.C_USER.ID,
                            FIRST_NAME = b.TEACH.C_USER.FIRST_NAME,
                            LAST_NAME = b.TEACH.C_USER.LAST_NAME,
                            MIDDLE_NAME = b.TEACH.C_USER.MIDDLE_NAME
                        }
                    },
                })
            });

            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }  
    }
}