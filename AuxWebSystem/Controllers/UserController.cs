using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Net.Http;
using AuxWebSystem.Models;
using AuxWebSystem.Helpers;
using AuxWebSystem.Filters;

namespace AuxWebSystem.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = false)]
        public ActionResult Index()
        {
            return View();
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult WebUserManager()
        {
            //web用户管理
            DataTable userTable;
            if (null == HttpRuntime.Cache[UserTableModel.CacheName])
            {
                UserTableModel usm = new UserTableModel();
                userTable = DataBaseHelper.getAllRecord(usm);
                HttpRuntime.Cache[UserTableModel.CacheName] = userTable;
            }
            else
            {
                userTable = HttpRuntime.Cache[UserTableModel.CacheName] as DataTable;
            }
            ViewBag.userTable = userTable;
            return View();
        }


        public ActionResult CurveJurisdictionManager()
        {
            //远程曲线权限管理
            
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
            if (null == modiUser.Password || "".Equals(modiUser.Password))
            {
                modiUser.Password = user.Password;
            }
            if (null == modiUser.NewPassword || "".Equals(modiUser.NewPassword))
            {
                modiUser.NewPassword = user.Password;
            }
            if (null == modiUser.OldPassword || "".Equals(modiUser.OldPassword))
            {
                modiUser.OldPassword = user.Password;
            }
            if (null == modiUser.ConfirmPassword || "".Equals(modiUser.ConfirmPassword))
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
                UserSecurityHelper.Logout();
                UserSecurityHelper.Login(new LoginModel(modiUser.LogName, modiUser.NewPassword));
                Session.Remove(ModifyUserTableModel.sessioName);
                HttpRuntime.Cache.Remove(UserTableModel.CacheName);
                UserSecurityHelper.Logout();
                UserSecurityHelper.Login(new LoginModel(modiUser.LogName, modiUser.NewPassword));
                ViewBag.message = "修改成功";
                return View("index");
            }
            catch (UserSecurityException e)
            {
                ViewBag.Error = e.Error;
                ViewBag.message = e.Message;
                return View("ModityUserPage");
            }
        }


        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult AdminModifyUser(FormCollection form)
        {
            var ust = Session[UserTableModel.SessionName] as UserTableModel;
            ViewBag.User = ust;
            var model = new ModifyUserTableModel();
            model.Password = ust.Password;
            model.NewPassword = ust.Password;
            model.OldPassword = ust.Password;
            model.ConfirmPassword = ust.Password;
            model.UserID = ust.UserID;
            model.LogName = ust.LogName;
            model.RealName = form["RealName"];
            if (null != form["Department"])
            {
                model.Department = int.Parse(form["Department"]);
            }
            model.Email = form["Email"];
            model.CellPhone = form["CellPhone"];
            try
            {
                UserSecurityHelper.ModifyUser(model, ust);
                HttpRuntime.Cache.Remove(UserTableModel.CacheName);
            }
            catch (UserSecurityException e)
            {
                ViewBag.Error = e.Error;
                ViewBag.User = model;
                Session[UserTableModel.SessionName] = model;
                ViewBag.Area = SystemParameterHelpers.getInstance().Select("Area");
                return View();
            }

            var helper = SystemParameterHelpers.getInstance();
            ViewBag.Area = helper.Select("Area");
            Response.Redirect("/User/WebUserManager");
            return null;
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult AdminModifyUserPage(int id)
        {
            UserTableModel ust = new UserTableModel();
            ust.UserID = id;
            DataBaseHelper.fillOneRecordByKeyToModel(ust);
            Session[UserTableModel.SessionName] = ust;
            ViewBag.User = ust;
            ViewBag.Area = SystemParameterHelpers.getInstance().Select("Area");
            return View("AdminModifyUser");
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


        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult RegisterPage()
        {
            //用户注册
            ViewBag.message = "用户添加";
            ViewBag.User = Session[UserTableModel.SessionRegister];
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
                HttpRuntime.Cache.Remove(UserTableModel.CacheName);
                Session.Remove(UserTableModel.SessionRegister);
            }
            catch (UserSecurityException e)
            {
                ViewBag.message = e.Message;
                ViewBag.Error = e.Error;
                Session[UserTableModel.SessionRegister] = mode;
                ViewBag.User = mode;
                return View("RegisterPage");
            }
            ViewBag.message = "添加成功";
            return View("Index");
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
                Response.Redirect("/Home");
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
                HttpRuntime.Cache.Remove(UserTableModel.CacheName);
            }
            catch (UserSecurityException e)
            {
                ViewBag.message = e.Message;
                return View("Index");
            }
            ViewBag.message = "删除成功";
            return View("Index");
        }

        [HttpPost]
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult UploadFile(FormCollection form)
        {
            ExcelHelper helper = new ExcelHelper();
            try
            {
                String fileName = helper.SaveExcelFile();
                DataTable dt = helper.getExcelData(fileName);

            }
            catch (ExcelHelperException e)
            {
                ViewBag.message = e.Message;
                return View("index");
            }
            return View("index");
        }


        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult DownloadExcel()
        {
            UserTableModel usm = new UserTableModel();
            DataTable datatable = DataBaseHelper.getAllRecord(usm);
            ExcelHelper helper = new ExcelHelper();
            try
            {
                String fileName = helper.ExportExcel(datatable);
                helper.UserDownloadFile(fileName);
            }
            catch (ExcelHelperException e)
            {
                ViewBag.message = e.Message;
                return View("index");
            }
            return null;
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


        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public ActionResult ResetPassword(int id)
        {
            try
            {
                UserSecurityHelper.ResetPassword(id);
                ViewBag.message = "重置密码成功";
                return View("index");
            }
            catch (UserSecurityException e)
            {
                ViewBag.message = e.Message;
                return View("index");
            }
        }



    }
}
