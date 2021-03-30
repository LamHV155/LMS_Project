using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace DAL.DAO
{
    public class SubjectsDAO
    {
        LMSProjectDBContext db = null;

        public SubjectsDAO() { db = new LMSProjectDBContext(); }
        public List<SUBJECT> getsubject()
        {
            return db.SUBJECTs.ToList();
        }
        public List<SUBJECT> getdetail(string id)
        {
            var sub = db.SUBJECTs.Where(x => x.ID == id).ToList();
            return sub.Select(a => new SUBJECT
            {
                ID = a.ID,
                NAME=a.NAME,
                DESCRIPTION=a.DESCRIPTION,
                FACULTY= new FACULTY
                {
                    ID=a.FACULTY.ID,
                    NAME=a.FACULTY.NAME,
                }
            }).ToList();
        }
        public bool delete (string id)
        {
            var model = db.SUBJECTs.Find(id);
            db.SUBJECTs.Remove(model);
            db.SaveChanges();
            return true;
        }
        public bool update (SUBJECT subject)
        {
            var model = db.SUBJECTs.Find(subject.ID);
            model.NAME = subject.NAME;
            model.DESCRIPTION = subject.DESCRIPTION;
            model.FACULTY_ID = subject.FACULTY_ID;
            db.SaveChanges();
            return true;
        }
        public bool add(SUBJECT subject)
        {
            db.SUBJECTs.Add(subject);
            db.SaveChanges();
            return true;
        }
        public List<SUBJECT> getcourseinsubjects(string id)
        {
            List<SUBJECT> sub = db.SUBJECTs.Where(x => x.ID == id).ToList();
            return sub.Select(a => new SUBJECT
            {
                ID = a.ID,
                NAME = a.NAME,
                COURSEs = a.COURSEs.Select(b => new COURSE
                {
                    ID =b.ID,
                    NAME=b.NAME,
                    DESCRIPTION=b.DESCRIPTION,
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
                }).ToList()
            }).ToList();
        }
        public int countsubjects()
        {
            var cntsubjects = db.SUBJECTs.ToList();
            return cntsubjects.Count();
        }
        public int CheckSubjects(string subject)
        {
            int sub = db.SUBJECTs.Count(x => x.NAME == subject);
            return sub;
        }
    }
}
