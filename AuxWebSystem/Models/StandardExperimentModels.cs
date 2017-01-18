using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuxWebSystem.Models
{
    public class StandardExperimentModels : TableModel
    {
        /// <summary>
        /// 选择某一条记录的SQL
        /// </summary>
        /// <returns></returns>
        public StandardExperimentModels()
        {
            TableName = "[StandardExperiment]";
        }
        public override string getRecordByKeySQL()
        {
            return @"SELECT * FROM " + TableName + "WHERE ExperimentNo = '" + ExperimentNo + "'";
        }
        //添加记录
        public override string getInsertSQL()
        {
            return @"INSERT INTO " + TableName + "(ExperimentName,ExperimentNo,StandardTestHours) VALUES ('" + ExperimentName + "','" + ExperimentNo + "','" + StandardTestHours + "')";
        }

        //删除记录
        public override string getDeleteSQL()
        {
            return @"DELETE FROM " + TableName + "WHERE ExperimentNo = '" + ExperimentNo + "'";
        }

        //修改记录
        public override string getUpdateSQL()
        {
            return @"UPDATE " + TableName + "SET ExperimentName = '" + ExperimentName + "' ,StandardTestHours = '" + StandardTestHours + "' WHERE ExperimentNo ='" + ExperimentNo + "'";
        }

        //显示全部
        public override string getAllRecordSQL()
        {
            return base.getAllRecordSQL();
        }

        /// <summary>
        /// 实验名称
        /// </summary>
        public String ExperimentName { set; get; }
        public String ExperimentNo { set; get; }
        public float StandardTestHours { set; get; }

    }
}