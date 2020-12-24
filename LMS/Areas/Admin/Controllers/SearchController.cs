﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Common;

namespace LMS.Areas.Admin.Controllers
{
    [CustomAuthorize("ADMIN")]
    public class SearchController : Controller
    {
        // GET: Admin/Search
        public ActionResult Index()
        {
            return View();
        }
    }
}