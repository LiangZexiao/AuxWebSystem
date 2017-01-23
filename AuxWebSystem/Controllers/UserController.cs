using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;
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
            ViewBag.userTable = UserSecurityHelper.GetUserDataTable();
            SystemParameterHelpers helper = SystemParameterHelpers.getInstance();
            DataRow[] departmentRows = helper.Select("Department");
            Dictionary<Object, Object> department = new Dictionary<Object, Object>();
            for (int i = 0; i < departmentRows.Length; i++)
            {
                department.Add(departmentRows[i]["ParameterNO"], departmentRows[i]["Value"]);
            }
            DataRow[] userTypeRows = helper.Select("UserType");
            Dictionary<Object, Object> userType = new Dictionary<Object, Object>();
            for (int i = 0; i < userTypeRows.Length; i++)
            {
                userType.Add(userTypeRows[i]["ParameterNO"], userTypeRows[i]["Value"]);
            }
            ViewBag.Area = helper.Select("Area");
            ViewBag.Department = department;
            ViewBag.UserType = userType;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">用户登录名</param>
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void CheckUserLogName(String id)
        {
            DataTable dt = UserSecurityHelper.GetUserDataTable();
            DataRow[] rows = dt.Select(String.Format(" LogName = '{0}' ", id));
            if (rows.Length > 0)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "姓名重复" }));
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">用户姓名</param>
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void CheckUserRealName(String id)
        {
            DataTable dt = UserSecurityHelper.GetUserDataTable();
            DataRow[] rows = dt.Select(String.Format(" RealName = '{0}' ", id));
            if (rows.Length > 0)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "姓名重复" }));
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
            }
        }

        /// <summary>
        /// 获得User的区域信息
        /// </summary>
        /// <param name="id"></param>
        public void getUserAreaInfomation(int id)
        {
            UserAreaModel uarea = new UserAreaModel(id);
            DataTable dt = DataBaseHelper.getRecordByKey(uarea);
            Response.Write(JsonConvert.SerializeObject(dt));
        }


        public void setUserAreaInformation(FormCollection form)
        {
            String userID = form["UserID"];
            String[] area = form["sub-checkbox"].Split(new Char[] { ',' });
            try{
                DataBaseHelper.Delete(new UserAreaModel(int.Parse(userID)));
                for (int i = 0; i < area.Length; i++)
                {
                    DataBaseHelper.Insert(new UserAreaModel(int.Parse(userID), int.Parse(area[i])));
                }
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
            }
            catch (Exception e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = e.Message }));
            }
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
        public void ModifyUser(FormCollection form)
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


            List<Object> serList = new List<Object>();
            serList.Add(new { LogName = user.LogName });
            serList.Add(new { CellPhone = user.CellPhone });
            serList.Add(new { Email = user.Email });
            serList.Add(new { RealName = user.Email });

            try
            {
                UserSecurityHelper.ModifyUser(modiUser, user);
                UserSecurityHelper.Logout();
                UserSecurityHelper.Login(new LoginModel(modiUser.LogName, modiUser.NewPassword));
                Session.Remove(ModifyUserTableModel.sessioName);
                UserSecurityHelper.ClearUserDataTableCache();
                UserSecurityHelper.Logout();
                UserSecurityHelper.Login(new LoginModel(modiUser.LogName, modiUser.NewPassword));
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功", error = new ModifyUserTableModel() }));
            }
            catch (UserSecurityException e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = e.Message, error = e.Error }));
            }
        }


        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void AdminModifyUser(FormCollection form)
        {
            var ust = Session[UserTableModel.SessionName] as UserTableModel;
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
            String ser;
            try
            {
                UserSecurityHelper.ModifyUser(model, ust);
                UserSecurityHelper.ClearUserDataTableCache();
                ser = JsonConvert.SerializeObject(new { state = 1, message = "成功" });
                Response.Write(ser);
            }
            catch (UserSecurityException e)
            {
                //ser = JsonConvert.SerializeObject( new  )
                ViewBag.Error = e.Error;
                ViewBag.User = model;
                Session[UserTableModel.SessionName] = model;
            }
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void AdminModifyUserPage(int id)
        {
            UserTableModel ust = new UserTableModel();
            ust.UserID = id;
            DataBaseHelper.fillOneRecordByKeyToModel(ust);
            Session[UserTableModel.SessionName] = ust;
            Response.Write(JsonConvert.SerializeObject(
                new
                {
                    UserID = ust.UserID,
                    LogName = ust.LogName,
                    RealName = ust.RealName,
                    Department = ust.Department,
                    UserType = ust.UserType,
                    Email = ust.Email,
                    CellPhone = ust.CellPhone
                }));
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
        public void Register(FormCollection form)
        {
            UserTableModel mode = new UserTableModel();
            mode.LogName = form["LogName"];
            mode.Email = form["Email"];
            mode.CellPhone = form["CellPhone"];
            mode.RealName = form["RealName"];
            String mess = "";

            int temp;
            if (int.TryParse(form["UserType"], out temp))
            {
                mode.UserType = temp;
            }
            else
            {
                mess += " 用户类型为空";
            }

            if (int.TryParse(form["Department"], out temp))
            {
                mode.Department = temp;
            }
            else
            {
                mess += " 部门为空";
            }

            if (mess.Length > 2)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = mess }));
                return;
            }

            try
            {
                UserSecurityHelper.Register(mode);
                UserSecurityHelper.ClearUserDataTableCache();
                Session.Remove(UserTableModel.SessionRegister);
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
            }
            catch (UserSecurityException e)
            {
                //TODO: e.Message 为空
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = e.Message, error = e.Error }));
            }
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
                TempData["loginError"] = e.Message;
                Response.Redirect("/Home");
            }
            return View("Index");
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void DeleteAccount(int id, String value)
        {
            try
            {
                UserTableModel tablemodel = new UserTableModel();
                tablemodel.UserID = id;
                tablemodel.LogName = value;
                UserSecurityHelper.DeleteAccount(tablemodel);
                UserSecurityHelper.ClearUserDataTableCache();
            }
            catch (UserSecurityException e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = e.Message }));
                return;
            }
            Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
        }

        [HttpPost]
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void UploadFile(FormCollection form)
        {
            ExcelHelper helper = new ExcelHelper();
            try
            {
                String fileName = helper.SaveExcelFile();
                DataTable dt = helper.getExcelData(fileName);
                helper.addDataTableToDatabase(dt);
                UserSecurityHelper.ClearUserDataTableCache();
            }
            catch (ExcelHelperException e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = e.Message }));
                return;
            }
            Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void GetExcelTemplate()
        {
            ExcelHelper excelHelper = new ExcelHelper();
            excelHelper.UserDownloadFile(ExcelHelper.templateName, "UserInfo.xlsx");
        }

        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void DownloadExcel()
        {
            ExcelHelper helper = new ExcelHelper();
            try
            {
                DataTable dt = helper.getUserTable();
                String fileName = helper.ExportExcel(dt);
                helper.UserDownloadFile(fileName, fileName);
            }
            catch (ExcelHelperException e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = e.Message }));
            }
            //Response.RedirectToRoute("/ExcelFile/636198609306488823.xlsx");
            Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
        }

        public void Logoff()
        {
            //用户登出
            UserSecurityHelper.Logout();
            Response.Redirect("/Home/Index");
        }

        //[HttpGet]
        //public ActionResult SearchName(FormCollection form)
        //{
        //    UserTableModel users = new UserTableModel();
        //    users.LogName = form["SearchName"];
        //    ViewBag.userTable = DataBaseHelper.getLikeRecord(users);
        //    return View("WebUserManager");
        //}

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
        public void ResetPassword(int id)
        {
            try
            {
                UserSecurityHelper.ResetPassword(id);
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "重置密码成功" }));
            }
            catch (UserSecurityException e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = e.Message }));
            }
        }
    }
}
