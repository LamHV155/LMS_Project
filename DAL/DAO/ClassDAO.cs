using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DAO;
using DAL.EF;

namespace DAL.DAO
{
    public class ClassDAO
    {
        LMSProjectDBContext db = null;

        public ClassDAO() { db = new LMSProjectDBContext(); }
        public List<CLASS> getclass()
        {
            return db.CLASSes.ToList();
        }
    }
}
