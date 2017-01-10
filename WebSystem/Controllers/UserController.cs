using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
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
            SqlConnection connection = DataBaseHelper.getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(UserTableModel.SelectAllSQL, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<UserTableModel> userList = new List<UserTableModel>();
            while (reader.Read())
            {
                userList.Add(new UserTableModel(reader));
            }

            ViewBag.userList = userList;
            reader.Close();
            connection.Close();
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
            SqlConnection connection = DataBaseHelper.getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(UserTableModel.SelectUserByIDSQL(id), connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                ViewBag.User = new UserTableModel(reader);
            }
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
            RegisterModel rgmode = new RegisterModel();
            rgmode.LogName = form["LogName"];
            rgmode.Area = int.Parse(form["Area"]);
            rgmode.Password = form["Password"];
            rgmode.ConfirmPassword = form["ConfirmPassword"];
            try
            {
                UserSecurityHelper.Register(rgmode);
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
            LoginModel model = new LoginModel();
            model.LogName = form["LogName"];
            model.Password = form["Password"];
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
                UserSecurityHelper.DeleteAccount(new SimpleUserModel(id,value));
            }
            catch (UserSecurityException e)
            {
                ViewBag.message = e.Message;
                return View("Index");
            }
            Response.Redirect("/User/WebUserManager");
            return null;
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
