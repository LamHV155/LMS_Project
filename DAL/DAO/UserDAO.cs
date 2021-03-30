using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using System.Data;
using System.Data.SqlClient;

namespace DAL.DAO
{
    public class UserDAO
    {
        LMSProjectDBContext db = null;
        public UserDAO() { db = new LMSProjectDBContext(); }

        public bool Login(string mail, string password)
        {
            int count = db.C_USER.Count(x => x.MAIL == mail && x.PASSWORD == password);
            if (count > 0)
            {
                return true;
            }
            else return false;
        }
        public C_USER GetUser(string mail)
        {
            return db.C_USER.SingleOrDefault(x => x.MAIL == mail);
        }

        public C_USER GetUserByID(string id)
        {
            C_USER user =  db.C_USER.First(u => u.ID == id);
            return new C_USER
            {
                ID = user.ID,
                FIRST_NAME = user.FIRST_NAME,
                MIDDLE_NAME = user.MIDDLE_NAME,
                LAST_NAME = user.LAST_NAME,
                DoB = user.DoB,
                MAIL = user.MAIL,
                PHONE_NO = user.PHONE_NO,
                LASTVISITDATE = user.LASTVISITDATE,
                CLASS = new CLASS
                {
                    ID = ((user.CLASS == null) ? null : user.CLASS.ID),
                    NAME = ((user.CLASS == null) ? null : user.CLASS.NAME),
                    MAJOR = ((user.CLASS == null) ? null : user.CLASS.MAJOR)

                },
                FACULTY = new FACULTY
                {
                    ID = user.FACULTY.ID,
                    NAME = user.FACULTY.NAME
                },
                ROLEs = user.ROLEs.Select(r => new ROLE
                {
                    ID = r.ID,
                    ROLE1 = r.ROLE1
                }).ToList(),

                //learns
                COURSEs = ((user.COURSEs != null) ? user.COURSEs.Select(s => new COURSE
                {
                    ID = s.ID,
                    NAME = s.NAME
                }).ToList() : null),

                //teaches
                TEACHES = ((user.TEACHES != null) ? user.TEACHES.Select(s => new TEACH
                {
                    COURSE = new COURSE
                    {
                        ID = s.COURSE_ID,
                        NAME = s.COURSE.NAME
                    }
                }).ToList() : null)
            };
        }

        public C_USER GetStudentsByIDWithSubmit(string id, string event_id)
        {
            C_USER u = db.C_USER.First(x => x.ID == id);
            C_USER student = new C_USER()
            {
                ID = u.ID,
                FIRST_NAME = u.FIRST_NAME,
                MIDDLE_NAME = u.MIDDLE_NAME,
                LAST_NAME = u.LAST_NAME,
                SUBMITs = u.SUBMITs.Where(s => s.EVENT_ID == event_id)
                                   .Select(s => new SUBMIT
                                   {
                                       ID = s.ID,
                                       LINK = s.LINK,
                                       TIME = s.TIME,
                                       ASSESSMENT = new ASSESSMENT 
                                       { 
                                           SCORE = ((s.ASSESSMENT == null)? null : s.ASSESSMENT.SCORE), 
                                           COMMENT = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.COMMENT)
                                       },
                                   }).ToList()
            };
            return student;

        } 


        public bool UpdatePhoneEmail(string id_user, string phone, string email)
        {
            C_USER user = db.C_USER.First(x => x.ID == id_user);
            try
            {
                user.PHONE_NO = phone;
                user.MAIL = email;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
           
        }

       public List<C_USER> GetListUserWithScore(string id_course)
        {
            COURSE course = db.COURSEs.First(x => x.ID == id_course);
            return course.C_USER.Select(u => new C_USER { 
                                                            ID = u.ID,
                                                            FIRST_NAME = u.FIRST_NAME,
                                                            MIDDLE_NAME = u.MIDDLE_NAME,
                                                            LAST_NAME = u.LAST_NAME,

                                                        }).ToList();
        }
        
    }
}
