using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using System.Data.Entity.Migrations;


namespace DAL.DAO
{
    public class AssessmentDAO
    {
        LMSProjectDBContext db = null;
        public AssessmentDAO() { db = new LMSProjectDBContext(); }
        
        public bool AddOrUpdate(int score, string cmt, string id_submit)
        {
            try
            {
                ASSESSMENT ass = new ASSESSMENT { SUBMIT_ID = id_submit, SCORE = score, COMMENT = cmt};
                db.ASSESSMENTs.AddOrUpdate(ass);
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
