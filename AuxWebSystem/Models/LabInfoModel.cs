using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuxWebSystem.Models
{

    public class LabInfoModel : TableModel
    {

        public static readonly String cacheName = "AreaStationCache";
        public LabInfoModel()
        {
            TableName = "AreaStation";
        }

        /// <summary>
        /// 区域
        /// </summary>
        public int Area { get; set; }
        /// <summary>
        /// 台号
        /// </summary>
        public int StationNO { get; set; }
        /// <summary>
        /// 台位
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 性质
        /// </summary>
        public string Propety { get; set; }
        /// <summary>
        /// 公司号
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 灯号
        /// </summary>
        public int LampNO { get; set; }
        /// <summary>
        /// 灯名
        /// </summary>
        public string LampText { get; set; }

        public override string getRecordByKeySQL()
        {
            return String.Format(@"SELECT * FROM {0} WHERE LampNO = '{1}' OR LampText = '{2}'", TableName, LampNO, LampText);
        }

        public override string getInsertSQL()
        {
            return @"INSERT INTO " + TableName + " ( Area, StationName, Property, LampNO, LampText ) VALUES ( '" + Area + "' , '" + StationName + "'  , '" + Propety + "' , '" + LampNO + "' , '" + LampText + "' )";
        }

        public override string getDeleteSQL()
        {
            return @"DELETE FROM " + TableName + " WHERE StationNO = '" + StationNO + "'";
        }



        //Area, StationName, Property, LampNO, LampText, StationNO
        public override string getAllRecordSQL()
        {
            return @"SELECT SArea.Value AS Area, AreaStation.StationName, 
                            AreaStation.Property, AreaStation.LampNO, 
                            AreaStation.LampText, AreaStation.StationNO 
                    FROM AreaStation 
                    JOIN SystemParameter AS SArea 
                    ON SArea.ParameterType = 'Area' AND SArea.ParameterNO = AreaStation.Area";
        }

    }
}