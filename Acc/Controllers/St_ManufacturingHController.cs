﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class St_ManufacturingHController : BaseController
    {
        // GET: St_Manufacturing
        public ActionResult Index()
        {
            return View();
        }
    }
}