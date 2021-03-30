using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace DAL.DAO
{
    public class TeachDAO
    {
        LMSProjectDBContext db = null;
        public TeachDAO() { db = new LMSProjectDBContext(); }
        public bool updateteach(TEACH Teach)
        {
            var teach = db.TEACHES.First(x => x.COURSE_ID == Teach.COURSE_ID);
            teach.USER_ID = Teach.USER_ID;
            db.SaveChanges();
            return true;
        }
        public bool addteach(TEACH Teach, COURSE Course)
        {
            TEACH tea = new TEACH();
            tea.COURSE_ID = Course.ID;
            tea.USER_ID = Teach.USER_ID;
            db.TEACHES.Add(tea);
            db.SaveChanges();
            return true;
        }
    }
}
