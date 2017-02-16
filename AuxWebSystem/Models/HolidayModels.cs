using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuxWebSystem.Models
{
    /*表中文名称	        节假日设定				
     * 表英文名称	        HolidaySetting				
     * 表用途	            记录节假日时间，管理员权限，精度为小时				
     * 所在位置	            sql 服务器				
     * 主键	                ID				
     * 备注	                日历显示，各个放假时间不能重叠，
     *                      设置预设周六日放假功能，
     *                      管理员可设置周几的几点到周几的几点为放假时间，
     *                      然后插入10年的。也可删除当前之后的所有放假时间，
     *                      让管理员重新设置		
     *                      
     * 中文名称	        英文字段	    类型	        是否可为空	外键关系	备注
     * 序号             ID	            int	            否		    自增
     * 放假开始时间	    StartTime	    Datetime	    否		
     * 放假结束时间	    EndTime	        Datetime	    否		
     * 放假原因	        HolidayReason	nvarchar(100)	否		
     */
    public class HolidayModels : TableModel
    {
        
        public HolidayModels()
        {
            TableName = @"HolidaySetting";
        }

        public override string getMyRecordSQL()
        {
            return String.Format(@"SELECT ID, StartTime, EndTime, HolidayReason FROM {0} WHERE ( StartTime = N'{1}' AND EndTime = N'{2}' ) ", TableName, StartTime, EndTime);
        }

        public override void FillData(System.Data.SqlClient.SqlDataReader reader)
        {
            if (!reader.IsDBNull(0))
            {
                ID = reader.GetInt32(0);
            }
            if (!reader.IsDBNull(1))
            {
                StartTime = reader.GetDateTime(1);
            }
            if (!reader.IsDBNull(2))
            {
                EndTime = reader.GetDateTime(2);
            }
            if (!reader.IsDBNull(3))
            {
                HolidayReason = reader.GetString(3);
            }
        }


        public override string getAllRecordSQL()
        {
            //return String.Format(@"SELECT * FROM {0} WHRER (StartTime='{1}')", TableName, StartTime);
            //return String.Format(@"SELECT * FROM {0} WHRER (StartTime='{1}'AND EndTime='{2}'AND Holiday='{3}')", TableName, StartTime.ToString("yyyy-mm-dd HH:mm:ss"), EndTime.ToString("yyyy-mm-dd HH:mm:ss"), HolidayReason);
            return String.Format(@"SELECT * FROM {0}", TableName);
        }

        public override string getRecordByKeySQL()
        {
            //return String.Format(@"SELECT * FROM {0} WHERE (StartTime<'{1}' AND EndTime>'{2}')OR (StartTime >'{3}'AND StartTime <'{4}')", TableName, StartTime.ToString("yyyy-mm-dd HH:mm:ss"), StartTime.ToString("yyyy-mm-dd HH:mm:ss"), StartTime.ToString("yyyy-mm-dd HH:mm:ss"), EndTime.ToString("yyyy-mm-dd HH:mm:ss"));
            return String.Format(@"SELECT * FROM {0} WHERE (StartTime < N'{1}' AND EndTime > N'{2}' ) OR ( StartTime >= N'{3}' AND StartTime < N'{4}' )", TableName, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), StartTime.ToString("yyyy-MM-dd HH:mm:ss"), StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //return String.Format(@"SELECT * FROM {0} WHERE (StartTime<'{1}' AND EndTime>'{2}')OR (StartTime >'{3}'AND StartTime <'{4}')", TableName, StartTime, StartTime, StartTime, EndTime);
        }
        public override string getInsertSQL()
        {
            return String.Format(@"INSERT INTO {0} ( StartTime, EndTime, HolidayReason) VALUES(N'{1}',N'{2}','{3}')", TableName, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), HolidayReason);
        }
        public override string getDeleteSQL()
        {
            //DELETE FROM Person WHERE LastName = 'Wilson' 
            return String.Format(@"DELETE FROM {0} WHERE ID = '{1}' ", TableName, ID);
        }

        public override string getLikeRecordSQL()
        {
            return @"SELECT ID, HolidayReason, StartTime, EndTime FROM HolidaySetting Where CONVERT(varchar(20),StartTime,120) LIKE '%" + Time + "%' OR CONVERT(varchar(20),EndTime,120) LIKE '%" + Time + "%'";
        }

        /// <summary>
        /// 年份-月份
        /// </summary>
        public string Time { get; set; } 

        /// <summary>
        /// 放假开始时间
        /// </summary>
        public DateTime StartTime { set; get; }

        /// <summary>
        /// 放假结束时间
        /// </summary>
        public DateTime EndTime { set; get; }

        /// <summary>
        /// 放假原因
        /// </summary>
        public String HolidayReason { set; get; }

        /// <summary>
        /// 序号
        /// </summary>
        public int ID { get; set; }


    }
}