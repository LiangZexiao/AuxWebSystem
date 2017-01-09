using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSystem.Controllers
{
    public class ParameterController : Controller
    {
        //
        // GET: /Parameter/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TimeParameter()
        {
            //时间参数

            return View();
        }

        public ActionResult Holiday()
        {
            //节假日


            return View();
        }

        public ActionResult PasswordManager()
        {
            //密码管理

            return View();
        }
    }
}
