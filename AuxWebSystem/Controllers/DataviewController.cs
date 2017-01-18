using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using AuxWebSystem.Models;
using AuxWebSystem.Helpers;

namespace AuxWebSystem.Controllers
{

    public class DataviewController : Controller
    {
        public ActionResult Index(FormCollection form)
        {
            return View();
        }

    }
}

