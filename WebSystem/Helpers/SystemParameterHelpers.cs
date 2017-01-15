using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.IO;

using WebSystem.Models;

namespace WebSystem.Helpers
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

        public static SystemParameterHelpers getInstance(){
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
                cache.Insert(cacheKey, dt, null, DateTime.Now.AddDays(5), TimeSpan.Zero );
                return dt;
            }
            else
            {
                return HttpRuntime.Cache[cacheKey] as DataTable;
            }
        }

        public DataRow[] Select(String Key, Object value)
        {
            return getDataTable().Select(String.Format(" {0} = '{1}' ",Key, value));
        }

    }
}