using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.IO;

using AuxWebSystem.Models;

namespace AuxWebSystem.Helpers
{
    public class SystemParameterHelpers
    {
        private static SystemParameterHelpers helper;
        private static readonly String cacheKey = "SystemParameterHelpers";
        private static SystemParameterModel systemModel;
        private static HashSet<String> ChineseSet;
        private static HashSet<String> EnglishSet;
        private static Dictionary<String, String> ParameterTypeDic;

        private SystemParameterHelpers()
        {
            if (null == systemModel)
            {
                systemModel = new SystemParameterModel();
            }
        }

        public static SystemParameterHelpers getInstance()
        {
            if (null == helper)
            {
                helper = new SystemParameterHelpers();
            }
            return helper;
        }

        /// <summary>
        /// 获得datatable
        /// </summary>
        /// <returns></returns>
        public DataTable getDataTable()
        {
            if (null == HttpRuntime.Cache[cacheKey])
            {
                Cache cache = HttpRuntime.Cache;
                DataTable dt = DataBaseHelper.getAllRecord(systemModel);
                cache.Insert(cacheKey, dt, null, DateTime.Now.AddDays(5), TimeSpan.Zero);
                return dt;
            }
            else
            {
                return HttpRuntime.Cache[cacheKey] as DataTable;
            }
        }

        private void removeDataTable()
        {
            if (null != HttpRuntime.Cache[cacheKey])
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
            ChineseSet = null;
            EnglishSet = null;
            ParameterTypeDic = null;
        }

        /// <summary>
        /// 用于向SystemParameter插入参数
        /// </summary>
        /// <param name="ParameterType">参数类型</param>
        /// <param name="Value">参数值</param>
        /// <returns>参数编号 ParameterNO</returns>
        public int Add(String ParameterType, String Value)
        {
            DataTable dt = getDataTable();
            //int ParameterNO = (int)dt.Compute("Max(Value)", String.Format(" ParameterType = {0} ", ParameterType));
            Object tempNo = dt.Compute("Max(ParameterNO)", String.Format(" ParameterType = '{0}' ", ParameterType));
            int ParameterNO = 0;
            if (!(DBNull.Value == tempNo))
            {
                ParameterNO = (int)tempNo;
            }
            SystemParameterModel sysmodel = new SystemParameterModel();
            sysmodel.ParameterType = ParameterType;
            sysmodel.ParameterNO = ParameterNO + 1;
            sysmodel.Value = Value;
            DataBaseHelper.Insert(sysmodel);
            removeDataTable();
            return ParameterNO+1;
        }


        /// <summary>
        /// 从SystemParameter删除数据
        /// </summary>
        /// <param name="ParameterType">参数类型</param>
        /// <param name="ParameterNO">参数编号</param>
        public void Delect(String ParameterType, int ParameterNO)
        {
            SystemParameterModel sysModel = new SystemParameterModel();
            sysModel.ParameterType = ParameterType;
            sysModel.ParameterNO = ParameterNO;
            DataBaseHelper.Delete(sysModel);
            removeDataTable();
        }

        /*
         * 参数类型	ParameterType	nvarchar(50)
         * 参数编号	ParameterNO	int
         * 参数值	Value	char(100)
         */

        /// <summary>
        /// 用于从SystemParameter获得数据
        /// </summary>
        /// <param name="ParameterType">参数类型  ParameterType</param>
        /// <returns></returns>
        public DataRow[] Select(String ParameterType)
        {
            return getDataTable().Select(String.Format(" ParameterType = '{0}' ", ParameterType));
        }

        /// <summary>
        /// 根据参数类型(ParameterType)和参数值(Value)获得参数编号(ParameterNO)
        /// </summary>
        /// <param name="ParameterType">参数类型</param>
        /// <param name="Value">参数值</param>
        /// <returns>参数编号</returns>
        public int Select(String ParameterType, String Value)
        {
            DataTable dt = getDataTable();
            DataRow[] rows = dt.Select(String.Format("ParameterType = '{0}' and  Value = '{1}' ", ParameterType, Value));
            if (rows.Length <= 0)
            {
                return -1;
            }
            else
            {
                return (int)rows[0]["ParameterNO"];
            }
        }

        /// <summary>
        /// 根据参数类型(ParameterType)和参数编号(ParameterNO)获得参数值(Value)
        /// </summary>
        /// <param name="ParameterType">参数类型</param>
        /// <param name="ParameterNO">参数编号</param>
        /// <returns>参数值 Value</returns>
        public String Select(String ParameterType, int ParameterNO)
        {
            DataTable dt = getDataTable();
            DataRow[] rows = dt.Select(String.Format("ParameterType = '{0}' and  ParameterNO = '{1}' ", ParameterType, ParameterNO));
            if (rows.Length <= 0)
            {
                return null;
            }
            else
            {
                return rows[0]["Value"] as String;
            }
        }

        /// <summary>
        /// 获得所有参数类型的中文名
        /// </summary>
        /// <returns></returns>
        public HashSet<String> getAllParameterTypeInChinese()
        {
            /*参数类型	    ParameterType
            *              ChineseName
            *参数编号	    ParameterNO
            *参数值	    Value
            *是否可修改	Revisable
            */
            if (null == ChineseSet)
            {
                DataTable dt = getDataTable();
                ChineseSet = new HashSet<String>();
                foreach (DataRow row in dt.Rows)
                {
                    ChineseSet.Add(row["ChineseName"].ToString());
                }

            }
            return ChineseSet;
        }

        /// <summary>
        /// 获得所有参数类型
        /// </summary>
        /// <returns></returns>
        public HashSet<String> getAllParameterTypeInEnglish()
        {
            /*参数类型	    ParameterType
            *              ChineseName
            *参数编号	    ParameterNO
            *参数值	    Value
            *是否可修改	Revisable
            */
            if (null == EnglishSet)
            {
                DataTable dt = getDataTable();
                EnglishSet = new HashSet<String>();
                foreach (DataRow row in dt.Rows)
                {
                    EnglishSet.Add(row["EnglishName"].ToString());
                }
            }
            return EnglishSet;
        }

        /// <summary>
        /// 获得参数类型的字典,ParameterType, ChineseName
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> getParameterTypeDictionary()
        {
            if (null == ParameterTypeDic)
            {
                ParameterTypeDic = new Dictionary<string, string>();
                DataTable dt = getDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    String key = row["ParameterType"].ToString();
                    if (!ParameterTypeDic.ContainsKey(key) && row["Revisable"].Equals(1))
                    {
                        ParameterTypeDic.Add(key, row["ChineseName"].ToString());
                    }
                }
            }
            return ParameterTypeDic;
        }
    }
}