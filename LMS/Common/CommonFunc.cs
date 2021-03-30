using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LMS.Models;
using DAL.EF;
using DAL.DAO;
using Newtonsoft.Json;

namespace LMS.Common
{
    public class CommonFunc
    {
        public CommonFunc() { }       

        public void SetSession(string userID=null, string semesterID=null, string courseID=null)
        {
            Session s = new Session();
            s.id_user = userID;
            s.id_course = courseID;
            s.id_semester = semesterID;
            RoleDAO rDao = new RoleDAO();
            s.role = rDao.GetRoles(userID).Select(x => x.ROLE1).ToList()[0];
            HttpContext.Current.Session[CommonConstants.SESSION] = s;
        }

        public string GetIdUserBySession()
        {
            Session s = new Session();
            s =  HttpContext.Current.Session[CommonConstants.SESSION] as Session;
            return (s == null) ? null : s.id_user;
        }
        
        public string GetIdSemesterBySession()
        {
            Session s = new Session();
            s = HttpContext.Current.Session[CommonConstants.SESSION] as Session;
            return s.id_semester;
        }
        public string GetIdCourseBySession()
        {
            Session s = new Session();
            s = HttpContext.Current.Session[CommonConstants.SESSION] as Session;
            return s.id_course;
        }

        public Session GetSession()
        {
            Session s = new Session();
            return HttpContext.Current.Session[CommonConstants.SESSION] as Session;
        }




        //************************Cookie***************************************************




        public void SetCookie()
        {
            HttpCookie httpCookie = new HttpCookie(GetIdUserBySession());
            httpCookie.Values.Add(CommonConstants.SESSION, HttpContext.Current.Session.SessionID);
            httpCookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        public bool CheckSessionInvalid()
        {
            try
            {
                if((HttpContext.Current.Session[CommonConstants.SESSION] as Session) != null)
                {
                    if (HttpContext.Current.Request.Cookies[GetIdUserBySession()].Value == (CommonConstants.SESSION + "=" + HttpContext.Current.Session.SessionID))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
            
        }

        public bool DelCookie()
        {
            if(HttpContext.Current.Request.Cookies[GetIdUserBySession()] != null)
            {
                HttpContext.Current.Response.Cookies[GetIdUserBySession()].Expires = DateTime.Now.AddDays(-1);
                return true;
            }
            return false;
        }

        
        public void SetCookieFileOfTopic(RootObject root)
        {
            string jsonFiles = JsonConvert.SerializeObject(root);
            HttpCookie httpCookie = new HttpCookie(CommonConstants.File);
            httpCookie.Value = jsonFiles;
            httpCookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }


        public bool DelCookieFile()
        {
            if (HttpContext.Current.Request.Cookies[CommonConstants.File] != null)
            {
                HttpContext.Current.Response.Cookies[CommonConstants.File].Expires = DateTime.Now.AddDays(-1);
                return true;
            }
            return false;
        }
        public List<File> GetFileFromCookie()
        {
          // List<File> files = new List<File>();
            string jsonFiles = HttpContext.Current.Request.Cookies[CommonConstants.File].Value;
            var  result = JsonConvert.DeserializeObject<RootObject>(jsonFiles);
            //List<string> fileName = result.File.Select(x => x.fileName).ToList();
            //List<string> linkFile = result.File.Select(x => x.linkFile).ToList();

            //for (var i = 0; i < fileName.Count(); i++)
            //{
            //    File file = new File(fileName[i], linkFile[i]);
            //    files.Add(file);
            //}
            return result.File;
        }

      
    }
}

