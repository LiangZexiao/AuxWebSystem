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
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = false)]
        public ActionResult ModifyUser(FormCollection form)
        {
            UserTableModel usm = (UserTableModel)Session[UserSecurityHelper.sessionName];
            ModifyUserTableModel tmp = new ModifyUserTableModel();
            tmp.UserID = usm.UserID;
            tmp.Password = form["Password"];
            tmp.NewPassword = form["Newpassword"];
            tmp.ConfirmPassword = form["ComfirmPassword"];
            tmp.Email = form["Email"];
            tmp.CellPhone = form["CellPhone"];
            if (null == tmp.Email)
            {
                tmp.Email = usm.Email;
            }
            if (null == tmp.CellPhone)
            {
                tmp.CellPhone = usm.CellPhone;
            }
            if (tmp.Password == usm.Password)
            {
                if (tmp.ConfirmPassword == tmp.NewPassword)
                {
                    if (DataBaseHelper.Update(tmp))
                    {
                        ViewBag.message = "修改成功";
                        UserSecurityHelper.Logout();
                        UserSecurityHelper.Login(new LoginModel(tmp.LogName, tmp.NewPassword));
                        return View("Index");
                    }
                    else
                    {
                        ViewBag.message = "修改失败，请检查网络，稍后再试";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "两次输入的新密码不同";
                    return View();
                }
            }
            else
            {
                ViewBag.message = "旧密码输入错误";
                return View();
            }
        }

        /// <summary>
        /// 修改用户信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ModityUserPage()
        {
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
        public HttpResponseMessage DownloadExcel()
        {
            UserTableModel usm = new UserTableModel();
            DataTable datatable = DataBaseHelper.getAllRecord(usm);
            String fileName = ExcelHelper.ExportExcel(datatable);
            return ExcelHelper.UserDownloadFile(fileName);
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
