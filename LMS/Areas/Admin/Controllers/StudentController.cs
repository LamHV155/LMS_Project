using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
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
    public class StudentController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LMSProjectDBContext"].ConnectionString);
        OleDbConnection Econ;

        LMSProjectDBContext db = new LMSProjectDBContext();
        // GET: Admin/Student
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetStudent(string name,int page, int pageSize)
        {
            var ListStudent = new InfoStudentDAO().getstudent().Select(x => new
            {
                ID = x.ID,
                FIRST_NAME = x.FIRST_NAME,
                LAST_NAME = x.LAST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                PHONE_NO = x.PHONE_NO,
                MAIL = x.MAIL,
                FACULTY = new
                {
                    ID = x.FACULTY.ID,
                    NAME = x.FACULTY.NAME
                },
                CLASS = new
                {
                    ID = x.CLASS.ID,
                    NAME = x.CLASS.NAME,
                    MAJOR = x.CLASS.MAJOR
                }
            });
            if (!string.IsNullOrEmpty(name))
            {
                ListStudent = ListStudent.Where(x => (x.LAST_NAME == name) || (x.MIDDLE_NAME == name) || (x.FIRST_NAME==name) ||(x.FACULTY.NAME==name));
            }
            int TotalRow = ListStudent.Count();
            var lstStudent = ListStudent.Skip((page - 1) * pageSize).Take(pageSize);
            return Json(new
            {
                total = TotalRow,
                data = lstStudent,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string id)
        {
            var info = new InfoStudentDAO().deletestudent(id);
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Detail(string idstudent)
        {
            var model = new InfoStudentDAO().detailstudent(idstudent).Select(x => new
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
                FACULTY = new { ID = x.FACULTY.ID, NAMEFACULTY = x.FACULTY.NAME },
                CLASS = new { ID = x.CLASS.ID, NAMECLASS = x.CLASS.NAME, MAJOR = x.CLASS.MAJOR }
            }).ToList();
            return Json(new
            {
                data = model,
                status = true
            });
        }
        public JsonResult GetClassInFaculty(string id)
        {
            var model = new FacultyDAO().getclassinfaculty(id).Select(x => new
            {
                CLASSes = x.CLASSes.Select(a => new { IDCLASS = a.ID, NAMECLASS = a.NAME, MAJORCLASS = a.MAJOR })
            });
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save(C_USER infostudent)
        {
            bool status = false;
            string message = string.Empty;
            if (infostudent.ID != null)
            {
                try
                {
                    var model = new InfoStudentDAO().updatestudent(infostudent);
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
        public JsonResult GetcoursebyID(string idstudent)
        {
            var sub = new InfoStudentDAO().getcoursebyID(idstudent).Select(x => new
            {
                ID = x.ID,
                FIRST_NAME = x.FIRST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                LAST_NAME = x.LAST_NAME,
                COURSE = x.COURSEs.Select(y => new
                {
                    IDCOURSE = y.ID,
                    NAMECOURSE = y.NAME,
                    DESCRIPTION = y.DESCRIPTION,
                    SEMESTER = new SEMESTER
                    {
                        ID = y.SEMESTER.ID,
                        TITLE = y.SEMESTER.TITLE,
                    }
                })
            });
            return Json(new
            {
                data = sub,
                status = true
            }, JsonRequestBehavior.AllowGet);
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
        }
        [HttpPost]
        public JsonResult UploadExcel(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                if (filename.EndsWith(".xlsx"))
                {
                    string filepath = "/Doc/" + filename;
                    file.SaveAs(Path.Combine(Server.MapPath("/Doc"), filename));
                    InsertExceldata(filepath, filename);
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = 1
                    });
                }
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}