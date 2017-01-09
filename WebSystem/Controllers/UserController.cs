using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSystem.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WebUserManager()
        {
            //web用户管理

            return View();
        }
        public ActionResult CurveJurisdictionManager()
        {
            //远程曲线权限管理

            return View();
        }
        
        public ActionResult Register()
        {
            //用户注册

            return View();
        }
        
        public ActionResult Login()
        {
            //用户登录

            return View();
        }        
        
        public ActionResult Logoff()
        {
            //用户登出

            return View();
        }
    }
}
