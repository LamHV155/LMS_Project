using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace DAL.DAO
{
    public class EventDAO
    {
        LMSProjectDBContext db = null;
        public EventDAO() { db = new LMSProjectDBContext(); }

        public bool InsertEvent(EVENT e)
        {
            try
            {
                db.EVENTs.Add(e);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateEvent(EVENT e)
        {
            try
            {
                db.EVENTs.AddOrUpdate(e);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteEvent(string id)
        {
            try
            {
                EVENT e = db.EVENTs.First(x => x.ID == id);
                db.EVENTs.Remove(e);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public List<EVENT> GetEventsOfCourse(string id_sub)
        {
            List<EVENT> listEvent = new List<EVENT>();

            COURSE sub = db.COURSEs.First(x => x.ID == id_sub);
            List<TOPIC> topics = sub.TOPICs.ToList();

            foreach(TOPIC topic in topics)
            {
                listEvent.AddRange(
                    topic.EVENTs.Select(ev => new EVENT
                    {
                        ID = ev.ID,
                        TITLE = ev.TITLE,
                        STARTDATE = ev.STARTDATE,
                        DEADLINE = ev.DEADLINE,
                        TOPIC_ID = ev.TOPIC_ID,
                        TOPIC = new TOPIC
                        {
                            ID = ev.TOPIC.ID,
                            TITLE = ev.TOPIC.TITLE,
                            DESCRIPTION = ev.TOPIC.DESCRIPTION,
                            COURSE_ID = ev.TOPIC.COURSE_ID,
                            COURSE = new COURSE
                            {
                                NAME = ev.TOPIC.COURSE.NAME,
                                DESCRIPTION = ev.TOPIC.COURSE.DESCRIPTION
                            }
                        },
                        SUBMITs = ev.SUBMITs.Select(s => new SUBMIT
                        {
                            ID = s.ID,
                            USER_ID = s.USER_ID,
                           ASSESSMENT = new ASSESSMENT
                           {
                               SCORE = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.SCORE),
                              COMMENT = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.COMMENT )
                           }
                        }).ToList()
                    }).ToList());
            }
            return listEvent;
        }

        public EVENT GetEventByID(string event_id)
        {
            EVENT ev = db.EVENTs.First(x => x.ID == event_id);
            return new EVENT
            {
                ID = ev.ID,
                TITLE = ev.TITLE,
                DESCRIPTION = ev.DESCRIPTION,
                STARTDATE = ev.STARTDATE,
                DEADLINE = ev.DEADLINE,
                TOPIC = new TOPIC
                {
                    ID = ev.TOPIC.ID,
                    TITLE = ev.TOPIC.TITLE,
                    DESCRIPTION = ev.TOPIC.DESCRIPTION,
                    COURSE_ID = ev.TOPIC.COURSE_ID,
                    COURSE = new COURSE
                    {
                        NAME = ev.TOPIC.COURSE.NAME,
                        DESCRIPTION = ev.TOPIC.COURSE.DESCRIPTION
                    }
                },
                SUBMITs = ev.SUBMITs.Select(s => new SUBMIT
                {
                    ID = s.ID,
                    USER_ID = s.USER_ID,
                    ASSESSMENT = new ASSESSMENT
                    {
                        SCORE = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.SCORE),
                        COMMENT = ((s.ASSESSMENT == null) ? null : s.ASSESSMENT.COMMENT)
                    }
                }).ToList()
            };
        }

    }
}
