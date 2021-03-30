using LMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [CustomAuthorize("ADMIN")]
    public class MenuSideBarController : Controller
    {
        // GET: Admin/MenuSideBar
        [ChildActionOnly]
        public ActionResult Index()
        {
            ViewBag.a = "A";
            return View();
        }
    }
}