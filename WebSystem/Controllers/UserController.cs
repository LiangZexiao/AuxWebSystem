using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Net.Http;
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
            return View();
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
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = false)]
        public ActionResult ModifyUser(FormCollection form)
        {
            UserTableModel user = (UserTableModel)Session[UserSecurityHelper.sessionName];
            ModifyUserTableModel modiUser = new ModifyUserTableModel();
            modiUser.LogName = user.LogName;
            modiUser.Password = form["Password"];
            modiUser.RealName = form["RealName"];
            modiUser.OldPassword = form["OldPassword"];
            modiUser.NewPassword = form["NewPassword"];
            modiUser.ConfirmPassword = form["ConfirmPassword"];
            modiUser.CellPhone = form["CellPhone"];
            modiUser.Email = form["Email"];
            if (null == modiUser.Password || "" == modiUser.Password)
            {
                modiUser.Password = user.Password;
            }
            if (null == modiUser.NewPassword || "" == modiUser.NewPassword)
            {
                modiUser.NewPassword = user.Password;
            }
            if (null == modiUser.OldPassword || "" == modiUser.OldPassword)
            {
                modiUser.OldPassword = user.Password;
            }
            if (null == modiUser.ConfirmPassword || "" == modiUser.ConfirmPassword)
            {
                modiUser.ConfirmPassword = user.Password;
            }
            modiUser.UserID = user.UserID;
            ViewBag.LogName = user.LogName;
            ViewBag.CellPhone = user.CellPhone;
            ViewBag.Email = user.Email;
            ViewBag.RealName = user.RealName;

            try
            {
                UserSecurityHelper.ModifyUser(modiUser, user);
            }
            catch (UserSecurityException e)
            {
                ViewBag.Error = e.Error;
                ViewBag.message = e.Message;
                return View("ModityUserPage");
            }
            UserSecurityHelper.Logout();
            UserSecurityHelper.Login(new LoginModel(modiUser.LogName, modiUser.NewPassword));
            Session.Remove(ModifyUserTableModel.sessioName);
            ViewBag.message = "修改成功";
            return View("index");
        }


        /// <summary>
        /// 修改用户信息页面
        /// </summary>
        /// <returns></returns>
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = false)]
        public ActionResult ModityUserPage()
        {
            UserTableModel user = (UserTableModel)Session[UserSecurityHelper.sessionName];
            ViewBag.LogName = user.LogName;
            ViewBag.CellPhone = user.CellPhone;
            ViewBag.Email = user.Email;
            ViewBag.RealName = user.RealName;
            return View();
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
            mode.RealName = form["RealName"];
            String dd = form["UserType"];
            mode.UserType = int.Parse(form["UserType"]);
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


        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void DownloadExcel()
        {
            UserTableModel usm = new UserTableModel();
            DataTable datatable = DataBaseHelper.getAllRecord(usm);
            //String fileName = ExcelHelper.ExportExcel(datatable);
            ExcelHelper.UserDownloadFile("636198609306488823.xlsx");
            //Response.RedirectToRoute("/ExcelFile/636198609306488823.xlsx");
        }


        public void Logoff()
        {
            //用户登出
            UserSecurityHelper.Logout();
            Response.Redirect("/Home/Index");
            return;
        }

        [HttpGet]
        public ActionResult SearchName(FormCollection form)
        {
            UserTableModel users = new UserTableModel();
            users.LogName = form["SearchName"];
            ViewBag.userTable = DataBaseHelper.getLikeRecord(users);
            return View("WebUserManager");
        }
    }
}
