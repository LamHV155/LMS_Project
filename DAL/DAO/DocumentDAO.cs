using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace DAL.DAO
{
    public class DocumentDAO
    {
        LMSProjectDBContext db = null;
        public DocumentDAO() { db = new LMSProjectDBContext(); }

        public bool InsertDocument(DOCUMENT doc)
        {
            try
            {
                db.DOCUMENTs.Add(doc);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateDocument(DOCUMENT doc)
        {
            try
            {
                db.DOCUMENTs.AddOrUpdate(doc);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                throw (ex);
                //return false;
            }
        }

        public bool DeleteDocument(string id)
        {
            try
            {
                DOCUMENT doc = db.DOCUMENTs.First(x => x.ID == id);
                db.DOCUMENTs.Remove(doc);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<string> GetTitleDocByTopicID(string topicid)
        {
            TOPIC topic = db.TOPICs.First(x => x.ID == topicid);
            List<string> titles = new List<string>();
            foreach(var doc in topic.DOCUMENTs)
            {
                titles.Add(doc.TITLE);
            }
            return titles;
        }

        public string GetTitle(string id)
        {
            DOCUMENT doc = db.DOCUMENTs.First(x =>x.ID == id);
            return doc.TITLE;
        }
        

        public string GetTopicIDByDoc(string id)
        {
            TOPIC topic = db.DOCUMENTs.Where(x => x.ID == id).Select(x => x.TOPIC).First();
            return topic.ID;
        }

    }
}
