using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebSystem.Models;
using WebSystem.Helpers;
namespace WebSystem.Controllers
{
    public class ParameterController : Controller
    {
        //
        // GET: /Parameter/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 时间参数
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public ActionResult TimeParameter(FormCollection Form)//添加项目
        {
            TimeParameterModels TimeParameter = new TimeParameterModels();
            TimeParameter.ExperimentName = char.Parse(Form["ExperimentName"]);
            TimeParameter.ExperimentTypeNo = char.Parse(Form["ExperimentTypeNo"]);
            TimeParameter.Duration = float.Parse(Form["Duration"]);
            String Str = @"Data Source=XZ-201508070039\LONGZHU;Integrated Security=True";
            SqlConnection connection = new SqlConnection(Str);
            connection.Open();
            String sql = "SELECT FROM StandardExperiment WHERE(ExperimentName= '" + TimeParameter.ExperimentName + "')";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                Reader.Close();
                connection.Close();
                Response.Write("添加失败，项目已存在");
                return View();

            }
            else
            {
                sql = "INSERT INTO StandardExperiment values('" + TimeParameter.ExperimentName + "','" + TimeParameter.ExperimentTypeNo + "','" + TimeParameter.Duration + "')";
                command = new SqlCommand(sql, connection);
            }
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                Response.Write("成功添加");
            }
            else
            {
                Response.Write("添加不成功");
            }
            connection.Close();
            return View();
        }

        public ActionResult ModityExperiment()
        {
            return View();
        }

        public ActionResult UpdateExperiment(FormCollection Form)//修改项目
        {
            TimeParameterModels TimeParameter = new TimeParameterModels();
            TimeParameter.ExperimentName = char.Parse(Form["ExperimentName"]);
            TimeParameter.ExperimentTypeNo = char.Parse(Form["ExperimentTypeNo"]);
            TimeParameter.Duration = float.Parse(Form["Duration"]);
            String Str = @"Data Source=XZ-201508070039\LONGZHU;Integrated Security=True";
            SqlConnection connection = new SqlConnection(Str);
            connection.Open();
            String sql = "UPDATE StandardExperiment set Duration='TimeParameter.Duration' WHERE (ExperimentName='TimeParameter.ExperimentName')";
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Close();
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                Response.Write("修改成功");
            }
            else
            {
                Response.Write("修改失败");
            }
            return View();
        }


        /// <summary>
        /// 节假日
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public ActionResult Holiday(FormCollection Form)//添加节假日
        {
            HolidayModels Holiday = new HolidayModels();
            Holiday.StartTime = DateTime.Parse(Form["StartTime"]);
            Holiday.EndTime = DateTime.Parse(Form["EndTime"]);
            Holiday.HolidayName = Char.Parse(Form["HolidayName"]);

            String Str = @"Data Source=XZ-201508070039\LONGZHU;Integrated Security=True";
            SqlConnection connection = new SqlConnection(Str);
            connection.Open();
            String sql = "SELECT * FROM HolidaySetting  WHERE (StartTime>=" + Holiday.StartTime + " AND StartTime<=" + Holiday.EndTime + ")OR (EndTime >=" + Holiday.StartTime + "AND EndTime <=" + Holiday.EndTime + ")";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                Reader.Close();
                connection.Close();
                ViewBag.error = "放假时间有重叠，您可以删除有冲突的，再设置";
                return View();
            }
            else
            {
                sql = "INSERT INTO HolidaySetting(StartTime,EndTime,HolidayName)values(' Holiday.StartTime','Holiday.EndTime',' Holiday.HolidayName')";
                command = new SqlCommand(sql, connection);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    Response.Write("添加成功");
                }
                else
                {
                    Response.Write("添加不成功");
                }
                connection.Close();

            }
            connection.Open();

            return View();
        }

        public ActionResult DelectHoliday(DateTime StartTime) //删除节假日
        {
            String str = @"Data Source=XZ-201508070039\LONGZHU;Integrated Security=True";
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            String sql = String.Format("DELETE FROM {0} WHERE={1}='{2}'", "HolidaySetting", "StartTime", StartTime);
            SqlCommand command = new SqlCommand(sql, connection);
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                Response.Write("删除成功");
            }
            else
            {
                Response.Write("删除不成功");
            }
            return View();
        }


    }
}
