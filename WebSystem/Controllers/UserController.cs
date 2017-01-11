using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WebSystem.Models;
using WebSystem.Helpers;
using WebSystem.Filters;

namespace WebSystem.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View("LoginPage");
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult WebUserManager()
        {
            //web用户管理
            UserTableModel usm = new UserTableModel();
            DataTable userTable = DataBaseHelper.getAllRecord(usm);
            ViewBag.userTable = userTable;
            return View();
        }


        public ActionResult CurveJurisdictionManager()
        {
            //远程曲线权限管理
            return View();
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult RegisterPage()
        {
            //用户注册
            ViewBag.message = "用户添加";
            return View();
        }

        /// <summary>
        /// 修改用户信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyUser(int id)
        {

            return View("Index");
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult Register(FormCollection form)
        {
            UserTableModel mode = new UserTableModel();
            mode.LogName = form["LogName"];
            mode.Email = form["Email"];
            mode.CellPhone = form["CellPhone"];
            try
            {
                UserSecurityHelper.Register(mode);
            }
            catch (UserSecurityException e)
            {
                ViewBag.message = e.Message;
                ViewBag.Error = e.Error;
                return View("RegisterPage");
            }
            Response.Redirect("/User/WebUserManager");
            return null;
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            LoginModel model = new LoginModel(form["LogName"], form["Password"]);
            try
            {
                UserSecurityHelper.Login(model);
            }
            catch (UserSecurityException e)
            {
                ViewBag.message = e.Message;
            }
            return View("Index");
        }
        

        public ActionResult LoginPage()
        {
            //用户登录
            return View();
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult DeleteAccount(int id, String value)
        {
            try
            {
                UserTableModel tablemodel = new UserTableModel();
                tablemodel.UserID = id;
                tablemodel.LogName = value;
                UserSecurityHelper.DeleteAccount(tablemodel);
            }
            catch (UserSecurityException e)
            {
                ViewBag.message = e.Message;
                return View("Index");
            }
            Response.Redirect("/User/WebUserManager");
            return null;
        }

        [HttpPost]
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult UploadFile(FormCollection form)
        {
            ExcelHelper helper = new ExcelHelper();
            helper.SaveFile();
            return View("index");
        }


        public void Logoff()
        {
            //用户登出
            UserSecurityHelper.Logout();
            Response.Redirect("/Home/Index");
            return;
        }
    }
}
