using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.DAO;
using DAL.EF;
using LMS.Common;
using LMS.Models;

namespace LMS.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            CommonFunc comf = new CommonFunc();
            string user_id = comf.GetIdUserBySession();

            UserDAO userDao = new UserDAO();
            C_USER user = userDao.GetUserByID(user_id);
            return View(user);
        }

        public ActionResult EditProfile()
        {
            Session session =  Session[CommonConstants.SESSION] as Session;
            
            UserDAO userDao = new UserDAO();
            C_USER user = userDao.GetUserByID(session.id_user);
            return View(user);
        }

        [HttpPost]
        public JsonResult UpdateInfo(string phone_no, string email)
        {
            Session session = Session[CommonConstants.SESSION] as Session;
            UserDAO user = new UserDAO();
            if (user.UpdatePhoneEmail(session.id_user, phone_no, email))
            {
                return Json(new { status = true });
            }
            return Json(new { status = false });
        }

        [HttpPost]
        public JsonResult Logout()
        {
            CommonFunc cf = new CommonFunc();
            if (cf.DelCookie())
            {
                Session.Clear();
                return Json(new { status = true});
            }
            return Json(new { status = false });
        }
    }
}