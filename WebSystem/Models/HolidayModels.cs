using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSystem.Models
{
    public class HolidayModels:TableModel
    {
        public override string getAllRecordSQL()
        {
            return String.Format(@"SELECT * FROM {0} WHERE (StartTime>='{1}' AND StartTime<='{2}')OR (EndTime >='{3}'AND EndTime <='{4}')" ,TableName,StartTime,EndTime,StartTime,EndTime);
        }
        public override string getInsertSQL()
        {
            return String.Format(@"INSERT INTO {0} (StartTime,EndTime,HolidayName) VALUES('{1}','{2}','{3}')", TableName, StartTime,EndTime,HolidayName);
        }
        public override string getDeleteSQL()
        {
            return String.Format(@"DELETE FROM {0} WHRER=(StartTime='{1}')",TableName,StartTime);
        }

        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
        public String HolidayName { set; get; }

       
    }
}