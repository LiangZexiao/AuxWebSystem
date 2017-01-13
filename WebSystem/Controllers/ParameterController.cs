using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data;
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
        public ActionResult TimeParameter()
        {
            return View();
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public ActionResult AddExperiment(FormCollection Form)
        {
            TimeParameterModels TimeParameter = new TimeParameterModels();
            TimeParameter.ExperimentName = Form["ExperimentName"];
            TimeParameter.ExperimentTypeNo = Form["ExperimentTypeNo"];
            TimeParameter.Duration = float.Parse(Form["Duration"]);
            
            if (DataBaseHelper.hasMyRecord(TimeParameter))
            {
                Response.Write("不能添加，项目已存在");
            }
            else {
                if (DataBaseHelper.Insert(TimeParameter))
                {
                    Response.Write("项目添加成功");
                }
                else {
                    Response.Write("未知错误导致项目不成功");
                }
            }
            return View();
           
        }
        public ActionResult ModityExperiment()
        {
            return View();
        }
        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public ActionResult UpdateExperiment(FormCollection Form)
        {
            TimeParameterModels TimeParameter = new TimeParameterModels();
            TimeParameter.ExperimentName = Form["ExperimentName"];
            TimeParameter.ExperimentTypeNo = Form["ExperimentTypeNo"];
            TimeParameter.Duration = float.Parse(Form["Duration"]);
            if (DataBaseHelper.Update(TimeParameter))
            {
                Response.Write("修改成功");
            }
            else {
                Response.Write("修改不成功");
            }
            return View();
            
        }

        public ActionResult HolidayManage(){

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
            Holiday.HolidayName = Form["HolidayName"];
            //DataTable datatable = DataBaseHelper.getAllRecord(Holiday);
            if (!DataBaseHelper.hasMyRecord(Holiday))
            {
                //TODO: do eomething
                if (DataBaseHelper.Insert(Holiday))
                {
                    Response.Write("添加成功");
                }
                else {
                    Response.Write("未知错误，添加不成功");
                }
            }
            else
            {
                Response.Write("放假时间有重叠，您需要重新设置放假时间");
            }

            return View();
        }

        public ActionResult DelectHoliday(DateTime StartTime) //删除节假日
        {
            HolidayModels Holiday = new HolidayModels();
            if (DataBaseHelper.Delete(Holiday))
            {
                Response.Write("删除成功");
            }
            else {
                Response.Write("删除不成功");
            }
            return View();
        }


    }
}
