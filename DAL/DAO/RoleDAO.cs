using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace DAL.DAO
{
    public class RoleDAO
    {
        LMSProjectDBContext db = null;

        public RoleDAO() { db = new LMSProjectDBContext(); }


        public List<ROLE> GetRoles(string user_id)
        {
            return db.ROLEs.Where(r => (r.C_USER.Where(u => u.ID == user_id).FirstOrDefault() != null) == true).ToList();
        }
        public List<ROLE> getrole()
        {
            return db.ROLEs.ToList();
        }
    }
}
