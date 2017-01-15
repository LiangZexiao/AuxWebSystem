using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebSystem.Helpers
{
    /// <summary>
    /// 用于拼接SQL语句
    /// </summary>
    public class SqlCommandHelper
    {
        public Dictionary<String, Object> DataMap { get; set; }
        public String WhereStr { get; set; }

        public String TableName { get; set; }

        public SqlCommandHelper(String tableName)
        {
            TableName = tableName;
            DataMap = new Dictionary<String, Object>();
        }

        public SqlCommandHelper()
        {
            DataMap = new Dictionary<String, Object>();
        }

        public SqlCommandHelper setTableName(String tableName)
        {
            TableName = tableName;
            return this;
        }

        public SqlCommandHelper Where(String command)
        {
            WhereStr = command;
            return this;
        }

        public SqlCommandHelper Add(String key, Object value)
        {
            if (null != value && !"".Equals(value) )
            {
                DataMap.Add(key, value);
            }
            return this;
        }


        public String Update()
        {
            //UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值
            var sql = new StringBuilder(String.Format("UPDATE {0} SET ", TableName));
            foreach (var str in DataMap)
            {
                sql.AppendFormat(@" {0} = '{1}', ", str.Key, str.Value);
            }
            sql.Append("$");
            sql.Replace(@", $", WhereStr);
            return sql.ToString();
        }

        public String Insert()
        {
            //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)
            var sbuder = new StringBuilder(String.Format("INSERT INTO {0} ( $ ) VALUES ( & )", TableName));
            foreach (var str in DataMap)
            {
                sbuder.Replace("$", str.Key + ", $");
                sbuder.Replace("&", "'" + str.Value + "', &");
            }
            sbuder.Replace(", $ )", " ) ");
            sbuder.Replace(", & )", " ) ");
            return sbuder.ToString();
        }


    }
}