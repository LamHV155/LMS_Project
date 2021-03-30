using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;


namespace DAL.DAO
{
    public class InfoStudentDAO
    {
        LMSProjectDBContext db = null;
        public InfoStudentDAO()
        {
            db = new LMSProjectDBContext();
        }
        public List<C_USER> getstudent()
        {
            ROLE role = db.ROLEs.Where(a => a.ROLE1 == "STUDENT").FirstOrDefault();
            List<C_USER> listteacher = role.C_USER.ToList();
            return listteacher.Select(x => new C_USER
            {
                ID = x.ID,
                FIRST_NAME = x.FIRST_NAME,
                LAST_NAME = x.LAST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                PHONE_NO = x.PHONE_NO,
                MAIL = x.MAIL,
                FACULTY = new FACULTY { ID = x.FACULTY.ID, NAME = x.FACULTY.NAME },
                CLASS=new CLASS { ID=x.CLASS.ID, NAME=x.CLASS.NAME, MAJOR=x.CLASS.MAJOR}
            }).ToList();
        }
        public bool deletestudent(string id)
        {
            var student = db.C_USER.Find(id);
            db.C_USER.Remove(student);
            db.SaveChanges();
            return true;
        }
        public List<C_USER> detailstudent(string IDstudent)
        {
            ROLE role = db.ROLEs.Where(a => a.ROLE1 == "STUDENT").FirstOrDefault();
            List<C_USER> listteacher = role.C_USER.ToList();
            return listteacher.Where(a => a.ID == IDstudent).Select(x => new C_USER
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
                FACULTY = new FACULTY { ID = x.FACULTY.ID, NAME = x.FACULTY.NAME },
                CLASS = new CLASS { ID = x.CLASS.ID, NAME = x.CLASS.NAME, MAJOR = x.CLASS.MAJOR }

            }).ToList();
        }
        public bool updatestudent(C_USER student)
        {
            var model = db.C_USER.Find(student.ID);
            model.FIRST_NAME = student.FIRST_NAME;
            model.LAST_NAME = student.LAST_NAME;
            model.MIDDLE_NAME = student.MIDDLE_NAME;
            model.PHONE_NO = student.PHONE_NO;
            model.DoB = student.DoB;
            model.SEX = student.SEX;
            model.MAIL = student.MAIL;
            model.PASSWORD = student.PASSWORD;
            model.FACULTY_ID = student.FACULTY_ID;
            model.CLASS_ID = student.CLASS_ID;
            db.SaveChanges();
            return true;
        }
        public List<C_USER> getcoursebyID(string ID)
        {
            List<C_USER> teacher = db.C_USER.Where(x => x.ID == ID).ToList();
            return teacher.Select(a => new C_USER
            {
                ID = a.ID,
                FIRST_NAME = a.FIRST_NAME,
                MIDDLE_NAME = a.MIDDLE_NAME,
                LAST_NAME = a.LAST_NAME,
                COURSEs = ((a.COURSEs==null) ? null : (a.COURSEs.Select(b => new COURSE
                {
                    ID = b.ID,
                    NAME = b.NAME,
                    DESCRIPTION=b.DESCRIPTION,
                    SEMESTER = new SEMESTER
                    {
                        ID = (b.SEMESTER.ID==null) ? null: (b.SEMESTER.ID),
                        TITLE = (b.SEMESTER.TITLE==null) ? null: (b.SEMESTER.TITLE),
                    }
                }).ToList()))
            }).ToList();
        }
        public int CountStudent()
        {
            ROLE role = db.ROLEs.Where(a => a.ROLE1 == "STUDENT").FirstOrDefault();
            List<C_USER> liststudent = role.C_USER.ToList();
            return liststudent.Count();
        }
    }
}
