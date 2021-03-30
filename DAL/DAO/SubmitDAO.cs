using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.StudentView;

namespace DAL.DAO
{
    public class SubmitDAO
    {
        LMSProjectDBContext db = null;
        public SubmitDAO() { db = new LMSProjectDBContext(); }
        
        public List<COURSE> GetSubmitAssessmentByStuAndCourseAndSem(string user_id)
        {
            C_USER stu = db.C_USER.Where(u => u.ID == user_id).First();
            //var model = stu.COURSEs.Where(x => x.ID == course_id).ToList();
            //return model.Select(x => new COURSE
            List<COURSE> course = stu.COURSEs.Select(x => new COURSE
            {
                ID = x.ID,
                NAME = x.NAME,
                TOPICs = ((x.TOPICs.Count == 0) ? null : (x.TOPICs.Select(t => new TOPIC
                {
                    EVENTs = ((t.EVENTs.Count() == 0) ? null : (t.EVENTs.Select(e => new EVENT
                    {
                        ID = e.ID,
                        TITLE = e.TITLE,
                        DEADLINE = e.DEADLINE,
                        SUBMITs = ((e.SUBMITs.FirstOrDefault(s => s.USER_ID == user_id) == null) ? null : (e.SUBMITs.Where(s => s.USER_ID == user_id)
                                                                                                                    .Select(s => new SUBMIT
                                                                                                                    {
                                                                                                                        ID = s.ID,
                                                                                                                        LINK = s.LINK,
                                                                                                                        TIME = s.TIME,
                                                                                                                        ASSESSMENT = new ASSESSMENT
                                                                                                                        {
                                                                                                                            SCORE = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.SCORE),
                                                                                                                            COMMENT = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.COMMENT)
                                                                                                                        }
                                                                                                                    }).ToList()))
                                                                                                                    
                    }).ToList()))
                }).ToList()))
            }).ToList();
           
            return course;
        }

        public COURSE GetCourseWithEventAndSubmmit(string user_id, string course_id)
        {
            C_USER stu = db.C_USER.Where(u => u.ID == user_id).First();
            COURSE course = stu.COURSEs.Where(x => x.ID == course_id).Select(x => new COURSE
            {
                ID = x.ID,
                NAME = x.NAME,
                C_USER = x.C_USER.Where(u => u.ID == user_id).Select(u => new C_USER { FIRST_NAME = u.FIRST_NAME, LAST_NAME = u.LAST_NAME, MIDDLE_NAME = u.MIDDLE_NAME}).ToList(),

                TOPICs = ((x.TOPICs.Count == 0) ? null : (x.TOPICs.Select(t => new TOPIC
                {
                    EVENTs = ((t.EVENTs.Count() == 0) ? null : (t.EVENTs.Select(e => new EVENT
                    {
                        ID = e.ID,
                        TITLE = e.TITLE,
                        DEADLINE = e.DEADLINE,
                        SUBMITs = ((e.SUBMITs.FirstOrDefault(s => s.USER_ID == user_id) == null) ? null : (e.SUBMITs.Where(s => s.USER_ID == user_id)
                                                                                                                    .Select(s => new SUBMIT
                                                                                                                    {
                                                                                                                        ID = s.ID,
                                                                                                                        LINK = s.LINK,
                                                                                                                        TIME = s.TIME,
                                                                                                                        ASSESSMENT = ((s.ASSESSMENT == null) ? null : new ASSESSMENT
                                                                                                                        {
                                                                                                                            SCORE = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.SCORE),
                                                                                                                            COMMENT = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.COMMENT)
                                                                                                                        })
                                                                                                                    }).ToList()))

                    }).ToList()))
                }).ToList()))
            }).FirstOrDefault();
            return course;
        }

        public List<COURSE> GetDeadlinebyStuAndCourseAndSem(string user_id)
        {
            C_USER stu = db.C_USER.Where(u => u.ID == user_id).First();
            List<COURSE> courses = stu.COURSEs.Select(x => new COURSE {
                ID = x.ID,
                NAME = x.NAME,
                TOPICs = ((x.TOPICs.Count == 0) ? null : (x.TOPICs.Select(t => new TOPIC
                {
                    EVENTs = ((t.EVENTs.Count() == 0) ? null : (t.EVENTs.Select(e => new EVENT
                    {
                        ID = e.ID,
                        TITLE = e.TITLE,
                        DESCRIPTION = e.DESCRIPTION,
                        DEADLINE = e.DEADLINE,
                        SUBMITs = ((e.SUBMITs.FirstOrDefault(s => s.USER_ID == user_id) == null) ? null : (e.SUBMITs.Where(s => s.USER_ID == user_id)
                                                                                                                    .Select(s => new SUBMIT
                                                                                                                    {
                                                                                                                        ID = s.ID
                                                                                                                    }).ToList()))
                    }).ToList()))
                }).ToList()))
            }).ToList(); 
            return courses;
        }
        public bool InsertSubmit(SUBMIT submit)
        {
            try
            {
                db.SUBMITs.Add(submit);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }  
        }
        public bool UpdateSubmit(SUBMIT submit)
        {
            try
            {
                var sb = db.SUBMITs.Find(submit.ID);
                sb.ID = submit.ID;
                sb.LINK = submit.LINK;
                sb.TIME = submit.TIME;
                sb.EVENT_ID = submit.EVENT_ID;
                sb.USER_ID = submit.USER_ID;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteSubmit(string id)
        {
            try
            {
                SUBMIT submit = db.SUBMITs.First(x => x.ID == id);
                db.SUBMITs.Remove(submit);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
