﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.StudentView;


namespace DAL.DAO
{
    public class TopicDAO
    {
        LMSProjectDBContext db = null;
        public TopicDAO() { db = new LMSProjectDBContext(); }

        public bool InsertTopic(TOPIC topic)
        {
            db.TOPICs.Add(topic);
            db.SaveChanges();
            return true;
        }

        public List<TOPIC> GetAllTopicOfTeacherCourse(string userId, string semId, string courseId)
        {
            //var topicsInSubTeachedByTeacher = db.C_USER.Where(x => x.ID == userId)
            //                    .Select(x => x.SUBJECTs1.Where(s => s.COURSE_ID == courseId && s.ID == subjectId)
            //                                            .Select(s => s.TOPICs)).ToList();

            C_USER teacher = db.C_USER.Where(u => u.ID == userId).First();
           TEACH teach = teacher.TEACHES.Where(t => t.COURSE.ID == courseId).Select(t => new TEACH { COURSE = t.COURSE}).First();
            List<TOPIC> topics = teach.COURSE.TOPICs.Select(t => new TOPIC
                {
                    ID = t.ID,
                    TITLE = t.TITLE,
                    DESCRIPTION = t.DESCRIPTION,
                    COURSE_ID = t.COURSE_ID,
                    DOCUMENTs = t.DOCUMENTs
                                    .Select(d => new DOCUMENT
                                    {
                                        ID = d.ID,
                                        TITLE = d.TITLE,
                                        DESCRIPTION = d.DESCRIPTION,
                                        LINK = d.LINK
                                    })
                                    .ToList(),

                    EVENTs = t.EVENTs
                                    .Select(e => new EVENT
                                    {
                                        ID = e.ID,
                                        TITLE = e.TITLE,
                                        DESCRIPTION = e.DESCRIPTION,
                                        STARTDATE = e.STARTDATE,
                                        DEADLINE = e.DEADLINE
                                    })
                                    .ToList()
                }).ToList();
            
            return topics;

        }

        public bool DeleteTopic(string id)
        {
            try
            {
                TOPIC topic = db.TOPICs.First(x => x.ID == id);
                db.TOPICs.Remove(topic);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public TOPIC GetTopicById(string id)
        {
            return db.TOPICs.First(x => x.ID == id);
        }
        public List<CourseDetailsView> GetCourseDetailByStuAndCourseAndSubject(string user_id, string course_id, string semester_id)
        {
            C_USER stu = db.C_USER.Where(u => u.ID == user_id).First();
            var model = (from a in db.COURSEs
                         join b in db.TOPICs
                         on a.ID equals b.COURSE_ID
                         join f in db.DOCUMENTs
                         on b.ID equals f.TOPIC_ID
                         join c in db.EVENTs
                         on b.ID equals c.TOPIC_ID
                         join d in db.SUBMITs
                         on c.ID equals d.EVENT_ID
                         where a.ID == course_id && stu.ID == d.USER_ID && a.SEMESTER_ID == semester_id
                         select new
                         {
                             courseID = a.ID,
                             courseName = a.NAME,
                             courseDescription = a.DESCRIPTION,
                             topicID = b.ID,
                             topicTitle = b.TITLE,
                             topicDescription = b.DESCRIPTION,
                             documentID = f.ID,
                             documentTitle = f.TITLE,
                             documentDescription = f.DESCRIPTION,
                             documentLink = f.LINK,
                             eventID = c.ID,
                             eventTitle = c.TITLE
                         }).AsEnumerable().Select(x => new CourseDetailsView()
                         {
                             courseID = x.courseID,
                             courseName = x.courseName,
                             courseDescription = x.courseDescription,
                             topicID = x.topicID,
                             topicTitle = x.topicTitle,
                             topicDescription = x.topicDescription,
                             documentID = x.documentID,
                             documentTitle = x.documentTitle,
                             documentDescription = x.documentDescription,
                             documentLink = x.documentLink,
                             eventID = x.eventID,
                             eventTitle = x.eventTitle
                         });
            return model.ToList();
        }
    }
}
