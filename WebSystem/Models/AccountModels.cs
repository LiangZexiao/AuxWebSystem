using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace WebSystem.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class SimpleUserModel
    {
        public SimpleUserModel(int id, String name)
        {
            UserID = id;
            LogName = name;
        }
        public int UserID { get; set; }
        public String LogName { get; set; }

        public String DeleteAccountSQL{
            get{
                //DELETE FROM 表名称 WHERE 列名称 = 值
                return String.Format("DELETE FROM {0} WHERE UserID = '{1}' AND LogName = '{2}'", UserTableModel.TableName, UserID, LogName);
            }
        }
        public String LoginStateSQL
        {
            get
            {
                //UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值
                return String.Format(@"UPDATE {0} SET State = '{1}' WHERE UserID = '{2}'", UserTableModel.TableName, 1, UserID);
            }
        }
        public String LogoutStateSql
        {
            get
            {
                return String.Format(@"UPDATE {0} SET State = '{1}' WHERE UserID = '{2}'", UserTableModel.TableName, 0, UserID);
            }
        }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public String LoginSQL
        {
            get
            {
                // SELECT * FROM 表名称
                return String.Format(@"SELECT * FROM {0} WHERE LogName = '{1}' AND Password = '{2}'", UserTableModel.TableName, LogName, Password);
            }
        }

    }

    public class RegisterModel : UserTableModel
    {
        public RegisterModel(): base(){

        }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        ///
        public string SelectLogNameSQL
        {
            get
            {
                //SELECT 列名称 FROM 表名称
                //WHERE 列 运算符 值
                return String.Format(@"SELECT LogName FROM {0} WHERE LogName = '{1}'", UserTableModel.TableName ,LogName);
            }
        }

        /// <summary>
        /// 获得注册的SQL语句
        /// </summary>
        public String RegisterSQL
        {
            get
            {
                //TODO: could be better
                //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)
                return String.Format(@"INSERT INTO {0} ( LogName, Password, Area, Email, CellPhone ) VALUES ( '{1}', '{2}', '{3}', '{4}', '{5}')", UserTableModel.TableName, LogName, Password, Area, Email, CellPhone );
            }
        }
    }
}
