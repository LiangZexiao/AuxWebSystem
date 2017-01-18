using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace AuxWebSystem.Helpers
{
    public class ProcedureHelper
    {
        /// <summary>
        /// 存储过程的名字
        /// </summary>
        public String ProcedureName { get; set; }

        /// <summary>
        /// 存储过程的参数
        /// </summary>
        private Dictionary<String, Object> Parameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName">存储过程的名字</param>
        public ProcedureHelper(String procedureName)
        {
            ProcedureName = procedureName;
            Parameter = new Dictionary<string, object>();
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ProcedureHelper Add(String key, Object value)
        {
            Parameter.Add(key, value);
            return this;
        }

        /// <summary>
        /// 得到存储过程的SQL语句
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder buder = new StringBuilder("exec ");
            buder.Append(ProcedureName);
            foreach (var para in Parameter)
            {
                buder.AppendFormat(@" @{0} = '{1}' ", para.Key, para.Value);
            }
            return buder.ToString();
        }


    }
}