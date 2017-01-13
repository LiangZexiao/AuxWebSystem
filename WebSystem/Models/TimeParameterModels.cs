using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSystem.Models
{   
    public class TimeParameterModels:TableModel
    {    
        /// <summary>
        /// 选择某一条记录的SQL
        /// </summary>
        /// <returns></returns>
        public override string getMyRecordSQL()
        {
            return String.Format(@"SELECT FROM {0} WHERE (ExperimentName='{1}')", TableName, ExperimentName);
        }
        public override string getInsertSQL()
        {
            //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)
            return String.Format(@"INSERT INTO {0} VALUES('{1}','{2}',{3}')", ExperimentName, ExperimentTypeNo,Duration);
        }
        public override string getUpdateSQL()
        {
            //UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值
            return String.Format(@"UPDATE {0} SET Duration='{1}'WHERE(ExperimentName='{2}')", TableName, Duration, ExperimentName);
        }
        
        /// <summary>
        /// 实验名称
        /// </summary>
        public String ExperimentName { set;get;}
        public String ExperimentTypeNo{ set;get;}
        public float Duration { set; get; }

    }
}