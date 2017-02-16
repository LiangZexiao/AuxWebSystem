using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuxWebSystem.Models
{

    public class LabInfoModel : TableModel
    {

        /* 表中文名称	    区域试验台				
         * 表英文名称	    AreaStation				
         * 表用途	        现有区域试验台信息				
         * 表位置	        服务器，本地				
         * 主键	            StationNO				
					
         * 中文名称	    英文字段	        类型	            是否可为空	键关系	备注
         * 台号	        StationNO	    int	            否		            自增
         * 区域	        Area	        int	            否		
         * 台位	        StationName	    nvarchar(200)	否		
         * 灯编号	    LampNO	        int	            否		
         * 灯字	        LampText	    nvarchar(200)	否		
         * 性质	        Property	    nvarchar(200)	否		
         * 数据资源	    Type	        int	            否		
         */

        public static readonly String cacheName = "AreaStationCache";
        public LabInfoModel()
        {
            TableName = "AreaStation";
        }

        public LabInfoModel(int stationNo)
        {
            StationNO = stationNo;
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

        public override string getMyRecordSQL()
        {
            return String.Format(@"SELECT StationNO, Area, StationName, LampNO, LampText, Property, Type FROM {0} WHERE LampNO = '{1}' OR LampText = '{2}'", TableName, LampNO, LampText);
        }
        /// <summary>
        /// key 为 Area
        /// </summary>
        /// <returns></returns>
        public override string getRecordByKeySQL()
        {
            return String.Format(@"SELECT StationNO, Area, StationName, LampNO, LampText, Property, Type FROM {0} WHERE Area = '{1}' ", TableName, Area );
        }

        public override void FillData(System.Data.SqlClient.SqlDataReader reader)
        {
            if (!reader.IsDBNull(0))
            {
                StationNO = reader.GetInt32(0);
            }
            if (!reader.IsDBNull(1))
            {
                Area = reader.GetInt32(1);
            }
            if (!reader.IsDBNull(2))
            {
                StationName = reader.GetString(2);
            }
            if (!reader.IsDBNull(3))
            {
                LampNO = reader.GetInt32(3);
            }
            if (!reader.IsDBNull(4))
            {
                LampText = reader.GetString(4);
            }
            if (!reader.IsDBNull(5))
            {
                Propety = reader.GetString(5);
            }
            if (!reader.IsDBNull(6))
            {
                Type = reader.GetInt32(6);
            }
        }

        public override string getInsertSQL()
        {
            return @"INSERT INTO " + TableName + " ( Area, StationName, Property, LampNO, LampText ) VALUES ( '" + Area + "' , '" + StationName + "'  , '" + Propety + "' , '" + LampNO + "' , '" + LampText + "' )";
        }

        public override string getDeleteSQL()
        {
            return String.Format(@"DELETE FROM {0} WHERE StationNO = {1} ", TableName, StationNO);
        }

        //Area, StationName, Property, LampNO, LampText, StationNO
        public override string getAllRecordSQL()
        {
            return @"SELECT SArea.Value AS Area, AreaStation.StationName, 
                            AreaStation.Property, AreaStation.LampNO, 
                            AreaStation.LampText, AreaStation.StationNO,
                            AreaStation.Area AS INTArea
                    FROM AreaStation 
                    JOIN SystemParameter AS SArea 
                    ON SArea.ParameterType = 'Area' AND SArea.ParameterNO = AreaStation.Area";
        }

    }
}