using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuxWebSystem.Models
{
    public class DataviewModel
    {
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 台号
        /// </summary>
        public string Station { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Datatime { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// 样机型号
        /// </summary>
        public string MachineType { get; set; }
        /// <summary>
        /// 开始读表时间
        /// </summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        /// 读表阶数时间
        /// </summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        /// 实际运行时间
        /// </summary>
        public DateTime validtime { get; set; }

        public double efficient { get; set; }
        
        public string DataCheckSQL
        {
            get
            {
                return string.Format(@"selcet * from table where Area='{0}'AND Station='{1}' AND DataTime='{2}'
                                         AND ItemNO= '{3}'AND MachineType='{4}'", Area, Station, Datatime, ItemNo, MachineType);
            }

        }

    }
}
