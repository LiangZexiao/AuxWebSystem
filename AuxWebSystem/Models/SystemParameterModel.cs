using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AuxWebSystem.Models
{
    /*
     * 表中文名称	    系统参数表				
     * 表英文名称	    SystemParameter				
     * 表用途	        系统中所有列表中的选项汇总集合				
     * 所在位置	        本地Access、FTP服务器Access和sql 服务器				
     * 主键	            ParameterType、ParameterNO				
     * 备注					
					
     * 中文名称	    英文字段	        类型	            是否可为空	外键关系	    备注
     * 参数类型	    ParameterType	nvarchar(50)	否		
     * 参数编号	    ParameterNO	    int	            否		
     * 参数值	    Value	        char(100)	    否		
     * 是否可修改	Revisable	    int	            否		                0:不可修改；1：可修改
     */
    public class SystemParameterModel : TableModel
    {

        /// <summary>
        /// 参数类型
        /// </summary>
        public String ParameterType { get; set; }

        /// <summary>
        /// 参数编号
        /// </summary>
        public int ParameterNO { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// 是否可修改
        /// 0:不可修改；1：可修改
        /// </summary>
        public int Revisable { get; set; }

        /// <summary>
        /// 插入
        /// 需要
        /// ParameterType
        /// ParameterNO
        /// Value
        /// </summary>
        /// <returns></returns>
        public override string getInsertSQL()
        {
            //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)
            return String.Format(@"INSERT INTO {0} ( ParameterType , ParameterNO, Value, Revisable ) VALUES ( '{1}', '{2}', '{3}', 1 )", TableName, ParameterType, ParameterNO, Value);
        }

        /// <summary>
        /// 删除
        /// 需要
        /// ParameterNO
        /// ParameterType
        /// </summary>
        /// <returns></returns>
        public override string getDeleteSQL()
        {
            //DELETE FROM 表名称 WHERE 列名称 = 值
            return String.Format(@"DELETE FROM {0} WHERE ParameterType = '{1}' AND ParameterNO = '{2}' AND Revisable <> '0' ", TableName, ParameterType, ParameterNO);
        }

        public SystemParameterModel()
        {
            TableName = "SystemParameter";
        }

        public override string getAllRecordSQL()
        {
            return @"SELECT Dic.ChineseName AS ChineseName, Sysp.ParameterType, Sysp.ParameterNO, Sysp.Value, Sysp.Value, Sysp.Revisable AS Revisable
                        FROM SystemParameter AS Sysp
                        LEFT JOIN EnglishDictionary as Dic
                        ON Sysp.ParameterType = Dic.EnglishName;";
        }

        /// <summary>
        /// 根据ParameterType和ParameterNO
        /// </summary>
        /// <returns></returns>
        public override string getMyRecordSQL()
        {
            //SELECT 列名称 FROM 表名称
            return String.Format(@"SELECT ParameterType, ParameterNO, Value, Revisable FROM {0} WHERE ParameterType = '{1}', ParameterNO = '{2}'", TableName, ParameterType, ParameterNO);
        }

        /// <summary>
        /// ParameterType 是主键
        /// </summary>
        /// <returns></returns>
        public override string getRecordByKeySQL()
        {
            return String.Format(@"SELECT ParameterType, ParameterNO, Value, Revisable FROM {0} WHERE ParameterType = '{1}'", TableName, ParameterType);
        }


    }
}