using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuxWebSystem.Models
{
    /*					
     * 表中文名称	        用户区域表				
     * 表英文名称	        UserArea				
     * 表用途	            查询用户区域				
     * 所在位置	            服务器				
     * 主键	                User,UserArea				
     * 
     * 中文名称     英文字段	    类型	    是否可为空	外键关系	    备注
     * 用户名	    UserID	    int	    否	        User	
     * 区域	        Area	    int	    否		
     */
    public class UserAreaModel : TableModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        /// 区域
        /// </summary>
        public int Area { set; get; }

        public UserAreaModel(int userID)
        {
            TableName = @"UserArea";
            UserID = userID;
        }

        public UserAreaModel(int userID, int area)
        {
            UserID = userID;
            Area = area;
            TableName = @"UserArea";
        }

        public override string getInsertSQL()
        {
            //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)
            return String.Format(@"INSERT INTO {0} ( UserID, Area ) VALUES ( '{1}', '{2}' )", TableName, UserID, Area );
        }

        public override string getAllRecordSQL()
        {
            return @"SELECT UserArea.UserID, UserArea.Area , SysPara.Value AS Value
                    FROM " + TableName + @" 
                    JOIN SystemParameter as SysPara
                    ON SysPara.ParameterType = 'Department' AND SysPara.ParameterNO = UserArea.Area";
        }

        public override string getUpdateSQL()
        {
            //UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值
            return String.Format(@"UPDATE {0} SET Area = '{1}' WHERE UserID = '{2}'", TableName, Area, UserID);
        }

        public override string getDeleteSQL()
        {
            //DELETE FROM 表名称 WHERE 列名称 = 值
            return String.Format(@"DELETE FROM {0} WHERE UserID = '{1}'", TableName, UserID);
        }
        
        public override string getMyRecordSQL()
        {
            //SELECT 列名称 FROM 表名称
            return String.Format(@"SELECT UserID, Area FROM {0} WHERE UserID = '{1}' ", TableName, UserID);
        }

        public override string getRecordByKeySQL()
        {
            return String.Format(@"SELECT UserArea.UserID, UserInfo.LogName, UserArea.Area, SysPara.Value AS Value
                        FROM {0}
                        JOIN SystemParameter as SysPara
                        ON SysPara.ParameterType = 'Department' AND SysPara.ParameterNO = UserArea.Area
                        AND UserArea.UserID = '{1}'
                        JOIN [User] as UserInfo
                        ON UserInfo.UserID = '{2}'
                        ", TableName, UserID, UserID);
        }


    }
}