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
        }

        /// <summary>
        /// 用于向SystemParameter插入参数
        /// </summary>
        /// <param name="ParameterType">参数类型</param>
        /// <param name="Value">参数值</param>
        public void Add(String ParameterType, String Value)
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

    }
}