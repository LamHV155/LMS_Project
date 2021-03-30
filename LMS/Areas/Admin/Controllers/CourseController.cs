using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.EF;
using DAL.DAO;
using System.Text;
using System.Web.Script.Serialization;
using LMS.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.IO;

namespace LMS.Areas.Admin.Controllers
{
    [CustomAuthorize("ADMIN")]
    public class CourseController : Controller
    {
        LMSProjectDBContext db = new LMSProjectDBContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LMSProjectDBContext"].ConnectionString);
        OleDbConnection Econ;

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetCourse(string name, int page, int pageSize)
        {
            var ListCourse = new CourseDAO().GetCOURSEs().Select(x => new
            {
                ID = x.ID,
                NAME = x.NAME,
                DESCRIPTION = x.DESCRIPTION,
                SUBJECT = new SUBJECT { FACULTY_ID=x.SUBJECT.FACULTY_ID}
            });
            if (!string.IsNullOrEmpty(name))
            {
                ListCourse = ListCourse.Where(x => x.NAME.Contains(name));
            }
            int TotalRow = ListCourse.Count();
            var lstCourse = ListCourse.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                total = TotalRow,
                data = lstCourse,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string id)
        {
            var model = new CourseDAO().deletecourse(id);
            return Json(new
            {
                status = true
            });
        }
        public JsonResult GetNameSemester()
        {
            var model = new SemesterDAO().getsemester().Select(x => new { ID = x.ID, TITLE = x.TITLE });
            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNameSubject()
        {
            var model = new SubjectsDAO().getsubject().Select(x => new { ID = x.ID, NAME = x.NAME });
            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }
        public string createID(string code)
        {
            var builder = new StringBuilder();
            builder.Append(code + DateTime.Now.ToString("yyyyMMddHHmmssff"));
            return builder.ToString();
        }
        public JsonResult Detail(string id)
        {
            var model = new CourseDAO().getdetail(id).Select(x => new
            {
                IDCOURSE = x.ID,
                NAMECOURSE = x.NAME,
                DESCRIPTION = x.DESCRIPTION,
                SEMESTER = (x.SEMESTER == null) ? null : (new SEMESTER
                {
                    ID = (x.SEMESTER.ID == null) ? null : (x.SEMESTER.ID),
                    TITLE = (x.SEMESTER.TITLE == null) ? null : (x.SEMESTER.TITLE)
                }),
                SUBJECT = (x.SUBJECT == null) ? null : (new SUBJECT
                {
                    ID = x.SUBJECT.ID,
                    NAME = x.SUBJECT.NAME
                }),
                TEACH = (x.TEACH == null) ? null : (new TEACH
                {
                    C_USER = (x.TEACH.C_USER == null) ? null : (new C_USER
                    {
                        ID = (x.TEACH.C_USER.ID == null) ? null : (x.TEACH.C_USER.ID),
                        FIRST_NAME = (x.TEACH.C_USER.FIRST_NAME == null) ? null : (x.TEACH.C_USER.FIRST_NAME),
                        LAST_NAME = (x.TEACH.C_USER.LAST_NAME == null) ? null : (x.TEACH.C_USER.LAST_NAME),
                        MIDDLE_NAME = (x.TEACH.C_USER.MIDDLE_NAME == null) ? null : (x.TEACH.C_USER.MIDDLE_NAME),
                        FACULTY = (x.TEACH.C_USER.FACULTY == null) ? null : (new FACULTY
                        {
                            ID = (x.TEACH.C_USER.FACULTY.ID == null) ? null : (x.TEACH.C_USER.FACULTY.ID),
                            NAME = (x.TEACH.C_USER.FACULTY.NAME == null) ? null : (x.TEACH.C_USER.FACULTY.NAME)
                        })
                    })
                })
            });
            return Json(new
            {
                data = model,
                status = true
            },JsonRequestBehavior.AllowGet);
        }
        public JsonResult InfoCourse(string idcourse = "AEES330233")
        {
            var course = new CourseDAO().InfoTeacherStudentInCourse(idcourse);
            return Json(new
            {
                data = course,
                status = true
            });
        }
        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);
        }
        private void InsertExceldata(string fileepath, string filename)
        {
            string fullpath = Server.MapPath("/Doc/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            using (SqlBulkCopy objbulk = new SqlBulkCopy(con))
            {
                objbulk.DestinationTableName = "_USER";
                objbulk.ColumnMappings.Add("ID", "ID");
                objbulk.ColumnMappings.Add("FIRST_NAME", "FIRST_NAME");
                objbulk.ColumnMappings.Add("LAST_NAME", "LAST_NAME");
                objbulk.ColumnMappings.Add("MIDDLE_NAME", "MIDDLE_NAME");
                objbulk.ColumnMappings.Add("PHONE_NO", "PHONE_NO");
                objbulk.ColumnMappings.Add("SEX", "SEX");
                objbulk.ColumnMappings.Add("DoB", "DoB");
                objbulk.ColumnMappings.Add("MAIL", "MAIL");
                objbulk.ColumnMappings.Add("PASSWORD", "PASSWORD");
                objbulk.ColumnMappings.Add("CLASS", "CLASS_ID");
                objbulk.ColumnMappings.Add("FACULTY", "FACULTY_ID");
                con.Open();
                objbulk.WriteToServer(dt);
                con.Close();
            }

            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
            {
                sqlBulkCopy.DestinationTableName = "USER_ROLE";
                sqlBulkCopy.ColumnMappings.Add("ID", "USER_ID");
                sqlBulkCopy.ColumnMappings.Add("ROLE", "ROLE_ID");
                con.Open();
                sqlBulkCopy.WriteToServer(dt);
                con.Close();
            }

            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
            {
                sqlBulkCopy.DestinationTableName = "LEARNS";
                sqlBulkCopy.ColumnMappings.Add("ID", "USER_ID");
                sqlBulkCopy.ColumnMappings.Add("COURSE", "COURSE_ID");
                con.Open();
                sqlBulkCopy.WriteToServer(dt);
                con.Close();
            }
        }
        public void UploadExcel(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                if (filename.EndsWith(".xlsx"))
                {
                    string filepath = "/Doc/" + filename;
                    file.SaveAs(Path.Combine(Server.MapPath("/Doc"), filename));
                    InsertExceldata(filepath, filename);
                }
            }
        }
        public JsonResult Save( HttpPostedFileBase file, string ID, string NAME, string DESCRIPTION, string SEMESTER_ID, string SUBJECT_ID, string USER_ID, string COURSE_ID)
        {
            COURSE Course = new COURSE();
            Course.ID = ID;
            Course.NAME = NAME;
            Course.DESCRIPTION = DESCRIPTION;
            Course.SEMESTER_ID = SEMESTER_ID;
            Course.SUBJECT_ID = SUBJECT_ID;

            TEACH Teach = new TEACH();
            Teach.USER_ID = USER_ID;
            Teach.COURSE_ID = COURSE_ID;

            bool status = false;
            string message = string.Empty;
            var course = new CourseDAO().CheckCouseIDExists(Course.ID);
            if (course == 1)
            {
                try
                {
                    var model = new TeachDAO().updateteach(Teach);
                    var cour = new CourseDAO().updatecourse(Course);
                    if (file != null)
                    {
                        UploadExcel(file);
                    }
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
                    //Course.ID = createID("COUR");
                    var cour = new CourseDAO().addcourse(Course);
                    var teach = new TeachDAO().addteach(Teach, Course);
                    UploadExcel(file);
                    status = true;
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
        public JsonResult DeleteStudent(string id)
        {
            var delstudent = new InfoStudentDAO().deletestudent(id);
            return Json(new
            {
                status = true
            });
        }
        public JsonResult GetTeacherInFaculy(string id)
        {
            var model = new FacultyDAO().getteacherinfaculty(id).Select(x => new
            {
                C_USER = x.C_USER.Select(a => new { IDTEA=a.ID, FIRST_NAME=a.FIRST_NAME, LAST_NAME=a.LAST_NAME, MIDDLE_NAME=a.MIDDLE_NAME})
            });
            return Json(new { data = model },JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubjectsInFaculty(string id)
        {
            var model = new FacultyDAO().getsubjectsinfaculty(id).Select(x => new
            {
                SUBJECTs = x.SUBJECTs.Select(a => new { IDSUBS = a.ID, NAMESUBS=a.NAME})
            });
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }
    }
}