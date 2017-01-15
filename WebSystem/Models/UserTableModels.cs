using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using WebSystem.Helpers;

namespace WebSystem.Models
{
    /*
     * 表英文名称	User					
     * 表用途	各类用户登录权限					
     * 表存在位置	服务器					
     * 主键	UserID					
     * 
     * 中文名称	    英文字段	        类型	            是否可为空	    外键关系	    默认	    备注
     * 用户编号	    UserID	        int	            否			    自增
     * 登录名	    LogName	        nvarchar(20)	否			
     * 姓名	        RealName	    nvarchar(20)	否			   
     * 密码	        Password	    nvarchar(20)	否		                    123456	  
     * 部门	        Department	    int	            是			SystemParameter
     * 用户类型	    UserType	    int	            否			                         0:超级管理员，1：web,2:曲线
     * 邮箱	        Email	        nvarchar(200)	是			
     * 手机	        CellPhone	    nvarchar(20）	是			
     * 最后登录时间	LastLoginTime	datetime	    是			
     */

    public class UserTableModel : TableModel
    {
        /// <summary>
        /// session 的名字
        /// </summary>
        public static readonly String SessionName = "UserTableModelSession";
        public static readonly String CacheName = "UserTableModelCache";

        /// <summary>
        /// 用户名
        /// 不可为空
        /// </summary>
        [Required]
        [Display(Name = "用户名")]
        public int UserID { get; set; }

        /// <summary>
        /// 登录名
        /// 不可为空
        /// </summary>
        [Required]
        [Display(Name = "登录名")]
        public String LogName { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public int Department { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 用户类型
        /// 0:超级管理员，1：web,2:曲线
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public String Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public String CellPhone { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }
        /// <summary>
        /// 超级用户权限
        /// 超级用户可删减其他用户
        /// </summary>

        public UserTableModel()
        {
            TableName = @"[User]";
        }


        public override string getRecordByKeySQL()
        {
            return String.Format(@"SELECT UserID, LogName,Department, Password, Email, CellPhone,RealName,UserType,LastLoginTime FROM {0} WHERE UserID = '{1}'", TableName, UserID);
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        public override String getUpdateSQL()
        {
            SqlCommandHelper helper = new SqlCommandHelper(TableName);
            helper.Add("Department", Department);
            helper.Add("Email", Email);
            helper.Add("CellPhone", CellPhone);
            helper.Add("RealName", RealName);
            return helper.Update();
            //UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值
            //return String.Format(@"UPDATE {0} SET Department = '{1}', Email = '{2}', CellPhone = '{3}' WHERE UserID = '{4}'", TableName, Department, Email, CellPhone);
        }

        public override string getDeleteSQL()
        {
            //DELETE FROM 表名称 WHERE 列名称 = 值     <>	不等于
            return String.Format("DELETE FROM {0} WHERE UserID = '{1}' AND LogName = '{2}' AND UserType <> '0' ", TableName, UserID, LogName);
        }

        public override string getInsertSQL()
        {
            SqlCommandHelper helper = new SqlCommandHelper(TableName);
            helper.Add("Email", Email);
            helper.Add("CellPhone", CellPhone);
            helper.Add("LogName", LogName);
            helper.Add("RealName", RealName);
            helper.Add("UserType", UserType);
            return helper.Insert();
            //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)
            //return String.Format(@"INSERT INTO {0} ( LogName, RealName, Email, CellPhone ) VALUES ( '{1}', '{2}', '{3}', '{4}')", TableName, LogName, RealName, Email, CellPhone);
        }


        public override string getAllRecordSQL()
        {
            return String.Format(@"SELECT UserID, LogName,Department, Email, CellPhone,RealName,UserType,LastLoginTime FROM {0}", TableName);
        }


        /// <summary>
        /// 根据用户ID获得用户信息的SQL语句
        /// </summary>
        public override String getMyRecordSQL()
        {
            return String.Format(@"SELECT UserID, LogName,Department, Password, Email, CellPhone,RealName,UserType,LastLoginTime FROM {0} WHERE LogName = '{1}'", TableName, LogName);
        }

        public override void FillData(SqlDataReader reader)
        {
            /*
             * UserID, LogName,Department, Password, Email, CellPhone,RealName,UserType,LastLoginTime
             */
            if (!reader.IsDBNull(0))
            {
                UserID = reader.GetInt32(0);
            }
            if (!reader.IsDBNull(1))
            {
                LogName = reader.GetString(1);
            }
            if (!reader.IsDBNull(2))
            {
                Department = reader.GetInt32(2);
            }
            if (!reader.IsDBNull(3))
            {
                Password = reader.GetString(3);
            }
            if (!reader.IsDBNull(4))
            {
                Email = reader.GetString(4);
            }
            if (!reader.IsDBNull(5))
            {
                CellPhone = reader.GetString(5);
            }
            if (!reader.IsDBNull(6))
            {
                RealName = reader.GetString(6);
            }
            if (!reader.IsDBNull(7))
            {
                UserType = reader.GetInt32(7);
            }
            if (!reader.IsDBNull(8))
            {
                LastLoginTime = reader.GetDateTime(8);
            }

        }

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns>如果是管理员，返回true. 否则,返回false</returns>
        public bool isAdminUser()
        {
            return UserType == 0;
        }

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="password">传入的密码</param>
        /// <returns>如果匹配,返回true. 否则,返回false</returns>
        public bool checkPassword(String password)
        {
            return password.Equals(Password);
        }

        /// <summary>
        /// 判断Email是否符合规范
        /// </summary>
        /// <param name="email">要判断的Email</param>
        /// <returns>如果符合规范，返回true， 否则，返回false</returns>
        public static bool checkEmail(String email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"[\w_-]+@[\w_-]+(\.[\w_-])+");
        }

        /// <summary>
        /// 判断Email是否符合规范
        /// </summary>
        /// <returns>如果符合规范，返回true， 否则，返回false</returns>
        public bool checkEmail()
        {
            return System.Text.RegularExpressions.Regex.IsMatch(Email, @"[\w_-]+@[\w_-]+(\.[\w_-])+");
        }

        /// <summary>
        /// 判断手机号码是否符合规范
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns>如果符合规范，返回true， 否则，返回false</returns>
        public static bool checkCellPhone(String phoneNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^1(3|4|5|7|8)\d{9}$");
        }

        /// <summary>
        /// 判断手机号码是否符合规范
        /// </summary>
        /// <returns>如果符合规范，返回true， 否则，返回false</returns>
        public bool checkCellPhone()
        {
            return System.Text.RegularExpressions.Regex.IsMatch(CellPhone, @"^1(3|4|5|7|8)\d{9}$");
        }
    }

    public class LoginModel : TableModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码</param>
        public LoginModel(String name, String password)
        {
            LogName = name;
            Password = password;
            TableName = "[User]";
        }

        public String LogName { get; set; }
        public String Password { get; set; }
        public DateTime LastLoginTime { get; set; }

        public override string getUpdateSQL()
        {
            //update YourTableName set [finishTime] = CAST('2014-09-01' AS datetime)
            return String.Format(@"UPDATE {0} SET LastLoginTime = getdate() WHERE LogName = '{1}'", TableName, LogName);
        }

        public override string getMyRecordSQL()
        {
            // SELECT * FROM 表名称
            return String.Format(@"SELECT * FROM {0} WHERE LogName = '{1}' AND Password = '{2}'", TableName, LogName, Password);
        }

        public override string getDeleteSQL()
        {
            //DELETE FROM 表名称 WHERE 列名称 = 值
            return String.Format("DELETE FROM {0} WHERE LogName = '{1}' AND AdminUser = '0'", TableName, LogName);
        }

    }


    public class ModifyUserTableModel : UserTableModel
    {

        public static readonly String sessioName = "modifyUser";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="oldPass">旧密码</param>
        /// <param name="newPass">新密码</param>
        /// <param name="confirmPass">确认新密码</param>
        public ModifyUserTableModel(int userId, String oldPass, String newPass, String confirmPass)
        {
            UserID = userId;
            OldPassword = oldPass;
            NewPassword = newPass;
            ConfirmPassword = confirmPass;
            TableName = "[User]";
        }

        public ModifyUserTableModel()
        {

        }


        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        public string ConfirmPassword { get; set; }


        public override string getMyRecordSQL()
        {
            return String.Format(@"SELECT * FROM {0} WHERE UserID = '{1}'", TableName, UserID);
        }

        public override string getUpdateSQL()
        {
            SqlCommandHelper helper = new SqlCommandHelper(TableName);
            helper.Add("Password", NewPassword);
            helper.Add("Email", Email);
            helper.Add("CellPhone", CellPhone);
            helper.Add("RealName", RealName);
            return helper.Where(String.Format(" WHERE UserID = '{0}' ", UserID)).Update();
        }

    }

}