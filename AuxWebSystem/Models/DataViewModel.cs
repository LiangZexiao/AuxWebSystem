using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuxWebSystem.Models;

namespace AuxWebSystem.Models
{
    public class DataviewModel : TableModel
    {
        public DataviewModel()
        {
            TableName = @"AreaStation";
        }
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }


        /// <summary>
        /// 台号
        /// </summary>
        public Int32 StationNO { get; set; }


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
        public DateTime StartTime { get; set; }


        /// <summary>
        /// 读表阶数时间
        /// </summary>
        public DateTime EndTime { get; set; }


        /// <summary>
        /// 实际运行时间
        /// </summary>
        public DateTime validtime { get; set; }

        public double efficient { get; set; }

        public string StationName { get; set; }

        public override string getMyRecordSQL()
        {
            return String.Format(@"SELECT StationNO FROM {0} WHERE (Area='{1}')", TableName, Area);
        }


        //获得一个区域内的所有试验台的台数
        /* public override string getRecordNumberSQL()
         {
             //Area在AreaStation  ; Time在MeterRecord;     
             //return String.Format(@"SELECE * FROM {0} WHERE (Area='{1}' AND StartTime>='{2}' AND EndTime<='{3}')",TableName,Area,StartTime,EndTime);
             return String.Format(@"SELECE StationNO FROM {0} WHERE (AreaStation.Area='{1}' AND Time>='{2}' AND Time<='{3}')", TableName, Area, StartTime, EndTime);
         }*/
    }

     class DataViewJson
    {

        public DataViewJson()
        {
            Area = "";
            AreaName = "";
            StationNo = "";
            StationName = "";
            Value = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="stationNumber">试验台号</param>
        /// <param name="stationName">试验台</param>
        /// <param name="value">值</param>
        public DataViewJson(String area,String areaName, String stationNumber, String stationName, Object value)
        {
            Area = area;
            AreaName = areaName;
            StationNo = stationNumber;
            StationName = stationName;
            Value = value;
        }
        /// <summary>
        /// 区域
        /// </summary>
        public String Area { get; set; }

        /// <summary>
        /// 试验台号
        /// </summary>
        public String StationNo { get; set; }

        /// <summary>
        /// 区域名字
        /// </summary>
        public String StationName { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        public Object Value { get; set; }

        public String AreaName { get; set; }
    }

}
