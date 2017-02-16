using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using AuxWebSystem.Models;


namespace AuxWebSystem.Helpers
{
    public class DataBaseHelper
    {
        //public DataSet dataset;
        //public DataTable dataTable;
        private DataBaseHelper()
        {

        }

        //private static readonly String connectionString = getConnectionString();

        /// <summary>
        /// 获得一个SQL连接
        /// </summary>
        /// <returns>新的SQL连接</returns>
        public static SqlConnection getSqlConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString);
        }

        //private static String getConnectionString()
        //{

        //    //TODO: we need it to get connection String from xml
        //    //Data Source=ALIY-DESKTOP;Initial Catalog=AUX_GROUP_CO;Integrated Security=True
        //    //String connectionString = @"Data Source=LENOVO-PC\SQLEXPRESS;Initial Catalog=AUX_GROUP_CO;Persist Security Info=True;User ID=sa;Password=123456";
        //    //String connectionString = @"Data Source=LENOVO-PC;Initial Catalog=AUX_GROUP_CO;Persist Security Info=True;User ID=guest;Password=guest";
        //     //String connectionString = @"Data Source=LENOVO-PC;Initial Catalog=AUX_GROUP_CO;Persist Security Info=True;User ID=guest;Password=123456";
        //   // String connectionString = @"Data Source=PC-201612302113;Initial Catalog=AUX_GROUP_CO;Integrated Security=True";
        //    //Data Source=LENOVO-PC;Initial Catalog=AUX_GROUP_CO;Persist Security Info=True;User ID=guest;Password=***********
        //    //String connectionString = @"Data Source=ALIY-DESKTOP;Initial Catalog=NEW_AUX_GROUP_CO;Integrated Security=True";
            
        //}

        /// <summary>
        /// 根据TableModel的AllRecordSQL
        /// 以DataTable形式返回数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>DataTable</returns>
        public static DataTable getAllRecord(TableModel model)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(model.getAllRecordSQL(), connection);
            dataAdapter.Fill(dt);
            connection.Close();
            return dt;
        }

        /// <summary>
        /// 根据TableModel的getMyRecordSQL填充一条信息
        /// 要使用本方法，必须要实现model里面的FillData() 接口
        /// </summary>
        /// <param name="model"></param>
        public static void fillOneRecordToModel(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getMyRecordSQL(), connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                model.FillData(reader);
            }
            reader.Close();
            connection.Close();
        }

        /// <summary>
        /// 根据TableModel的getRecordByKeySQL填充一条信息
        /// 要使用本方法，必须要实现model里面的FillData() 接口
        /// </summary>
        /// <param name="model"></param>
        public static void fillOneRecordByKeyToModel(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getRecordByKeySQL(), connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                model.FillData(reader);
            }
            reader.Close();
            connection.Close();
        }

        /// <summary>
        /// 根据TableModel的UpdateSQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getUpdateSQL(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result >= 1;
        }

        /// <summary>
        /// 根据TableModel的getInsertSQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns>执行结果</returns>
        public static bool Insert(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getInsertSQL(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result >= 1;
        }

        /// <summary>
        /// 根据TableModel的getDeleteSQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Delete(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getDeleteSQL(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result >= 1;
        }

        /// <summary>
        /// 根据TableModel的getRecordNumberSQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int RecordNumber(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getRecordNumberSQL(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }

        /// <summary>
        /// 根据TableModel的getMyRecordSQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool hasMyRecord(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getMyRecordSQL(), connection);
            SqlDataReader reader = command.ExecuteReader();
            bool succ = reader.Read();
            reader.Close();
            connection.Close();
            return succ;
        }



        /// <summary>
        /// 根据TableModel的getRecordByKeySQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns>是否有以key为值的量</returns>
        public static bool hasMyKeyRecord(TableModel model)
        {
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(model.getRecordByKeySQL(), connection);
            SqlDataReader reader = command.ExecuteReader();
            bool succ = reader.Read();
            reader.Close();
            connection.Close();
            return succ;
        }

        /// <summary>
        /// 根据TableModel的getRecordByKeySQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns>DataTable</returns>
        public static DataTable getRecordByKey(TableModel model)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(model.getRecordByKeySQL(), connection);
            dataAdapter.Fill(dt);
            connection.Close();
            return dt;
        }

        /// <summary>
        /// 根据TableModel的getMyRecordSQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns>DataTable</returns>
        public static DataTable getMyRecord(TableModel model)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(model.getMyRecordSQL(), connection);
            dataAdapter.Fill(dt);
            connection.Close();
            return dt;
        }

        /// <summary>
        /// 根据TableModel的getLikeRecordSQL执行SQL语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns>DataTable</returns>
        public static DataTable getLikeRecord(TableModel model)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(model.getLikeRecordSQL(), connection);
            dataAdapter.Fill(dt);
            connection.Close();
            return dt;
        }

        /// <summary>
        /// 这个方法是直接使用SQL语句来得到DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable getDataTableBySql(String sql)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = getSqlConnection();
            connection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);
            dataAdapter.Fill(dt);
            connection.Close();
            return dt;
        }

    }
}