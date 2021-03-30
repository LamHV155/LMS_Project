using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
namespace DAL.DAO
{
    public class SemesterDAO
    {
        LMSProjectDBContext db = null;

        public SemesterDAO() { db = new LMSProjectDBContext(); }

        public List<SEMESTER> GetSEMESTERs()
        {
            return db.SEMESTERs.ToList();
        }
        public List<SEMESTER> getsemester()
        {
            return db.SEMESTERs.ToList();
        }
        public List<SEMESTER> getdetail(string id)
        {
            return db.SEMESTERs.Where(x => x.ID == id).ToList();
        }
        
        public bool deletesemester(string id)
        {
            var semeste = db.SEMESTERs.First(x => x.ID == id);
            db.SEMESTERs.Remove(semeste);
            db.SaveChanges();
            return true;
        }
        public bool updatesemester(SEMESTER semester)
        {
            var model = db.SEMESTERs.Find(semester.ID);
            model.TITLE = semester.TITLE;
            model.DESCRIPTION = semester.DESCRIPTION;
            model.START = semester.START;
            model.START=semester.END_SEM;
            db.SaveChanges();
            return true;
        }
        public bool addsemester(SEMESTER semester)
        {
            db.SEMESTERs.Add(semester);
            db.SaveChanges();
            return true;
        }

        public SEMESTER GetSemesterByID(string id)
        {
            return db.SEMESTERs.First(x => id == x.ID);
        }


    }
}
