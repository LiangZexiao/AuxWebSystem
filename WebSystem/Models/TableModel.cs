using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace WebSystem.Models
{
    public abstract class TableModel
    {
        /// <summary>
        /// 表的名字
        /// </summary>
        public static String TableName;

        /// <summary>
        /// 获得全部记录的SQL
        /// </summary>
        public virtual String getAllRecordSQL()
        {
            return @"SELECT * FROM " + TableName;
        }

        /// <summary>
        /// 选择某一条记录的SQL
        /// </summary>
        /// <returns></returns>
        public virtual String getMyRecordSQL()
        {
            throw new MethodAccessException("方法: getMyRecordSQL() 没有实现");
        }

        /// <summary>
        /// 根据Key找纪录
        /// </summary>
        /// <returns></returns>
        public virtual String getRecordByKeySQL()
        {
            throw new MethodAccessException("方法: getRecordByKeySQL() 没有实现");
        }

        /// <summary>
        /// 更新记录的SQL
        /// </summary>
        public virtual String getUpdateSQL()
        {
            throw new MethodAccessException("方法: getUpdateSQL() 没有实现");
        }

        /// <summary>
        /// 删除记录的SQL
        /// </summary>
        /// <returns></returns>
        public virtual String getDeleteSQL()
        {
            throw new MethodAccessException("方法: getDeleteSQL() 没有实现");
        }

        /// <summary>
        /// 增加记录的SQL
        /// </summary>
        public virtual String getInsertSQL()
        {
            throw new MethodAccessException("方法: getInsertSQL() 没有实现");
        }

        /// <summary>
        /// 获得记录条数
        /// </summary>
        public virtual String getRecordNumberSQL()
        {
            //SELECT COUNT(*) FROM table_name
            //COUNT(*) 函数返回表中的记录数：
            return @"SELECT COUNT(*) FROM " + TableName;
        }

        /// <summary>
        /// 根据SqlDataReader填充数据
        /// </summary>
        /// <param name="reader"></param>
        public virtual void FillData(SqlDataReader reader)
        {
            throw new MethodAccessException("方法: FillData() 没有实现");
        }

    }
}