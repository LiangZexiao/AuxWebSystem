using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data;
using AuxWebSystem.Models;
using AuxWebSystem.Helpers;

namespace AuxWebSystem.Controllers
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
        /// 系统参数设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemParameterSetting()
        {
            ViewBag.SystemTable = SystemParameterHelpers.getInstance().getDataTable();
            return View();
        }

        /// <summary>
        /// 添加系统参数
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult SystemParameterAdding(FormCollection form)
        {
            /*
     * 参数类型	    ParameterType	nvarchar(50)
     * 参数编号	    ParameterNO	    int
     * 参数值	    Value	        char(100)
     * 是否可修改	Revisable	    int
     */
            String parameterType = form["ParameterType"];
            String value = form["Value"];
            SystemParameterHelpers helper = SystemParameterHelpers.getInstance();
            helper.Add(parameterType, value);
            ViewBag.SystemTable = helper.getDataTable();
            return View("SystemParameterSetting");
        }

        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="id">参数编号 ParameterNO</param>
        /// <param name="value">参数类型 ParameterType</param>
        /// <returns></returns>
        public ActionResult SystemParameterDelete(int id, String value)
        {
            SystemParameterHelpers helper = SystemParameterHelpers.getInstance();
            helper.Delect(value, id);
            ViewBag.SystemTable = SystemParameterHelpers.getInstance().getDataTable();
            return View("SystemParameterSetting");
        }



        /// <summary>
        /// 显示时间参数
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public ActionResult TimeParameter()
        {
            DataTable dt = DataBaseHelper.getAllRecord(new StandardExperimentModels());
            ViewBag.Standard = dt;
            return View();
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public ActionResult AddExperiment(FormCollection Form)
        {
            StandardExperimentModels TimeParameter = new StandardExperimentModels();
            TimeParameter.ExperimentName = Form["ExperimentName"];
            TimeParameter.ExperimentNo = Form["ExperimentNo"];
            if (DataBaseHelper.hasMyKeyRecord(TimeParameter))
            {
                ViewBag.Delect = "亲，试验类型编号不能重复哦!!!";
                return View("TimeParameter");
            }
            if (null != Form["StandardTestHours"])
            {
                TimeParameter.StandardTestHours = float.Parse(Form["StandardTestHours"]);
            }
            else
            {
                return View();
            }
            if (DataBaseHelper.Insert(TimeParameter))
            { }
            else
            {
                return View("TimeParameter");
            }
            return View("TimeParameter");

        }


        public ActionResult Delect(string id)
        {
            StandardExperimentModels SE = new StandardExperimentModels();
            SE.ExperimentNo = id;
            if (DataBaseHelper.Delete(SE))
            {
                ViewBag.Delect = "删除成功";
            }
            else
            {
                ViewBag.Delect = "删除失败";
                return View("TimeParameter");
            }
            return View("TimeParameter");
        }



        public ActionResult Holiday()
        {
            DataTable dt = DataBaseHelper.getAllRecord(new HolidayModels());
            ViewBag.table = dt;
            return View();
        }

        public ActionResult Add(FormCollection Form)
        {
            HolidayModels Holiday = new HolidayModels();
            if (null != Form["StartTime"])
            {
                Holiday.StartTime = DateTime.Parse(Form["StartTime"]);
            }
            if (null != Form["EndTime"])
            {
                Holiday.EndTime = DateTime.Parse(Form["EndTime"]);
            }

            Holiday.HolidayReason = Form["HolidayReason"];
            //DataTable datatable = DataBaseHelper.getAllRecord(Holiday);
            if (!DataBaseHelper.hasMyKeyRecord(Holiday))
            {
                //TODO: do eomething
                if (DataBaseHelper.Insert(Holiday))
                {
                    ViewBag.message = "添加成功";
                }
                else
                {
                    ViewBag.message = "未知错误，添加不成功";
                }
            }
            else
            {
                ViewBag.message = "放假时间有重叠，您需要重新设置放假时间";
            }
            return View();
        }
        /// <summary>
        /// 节假日
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        public ActionResult AddHoliday()//添加节假日
        {

            return View();
        }

        public ActionResult DeleteHoliday(int id) //删除节假日
        {
            HolidayModels Holiday = new HolidayModels();
            Holiday.ID = id;
            if (DataBaseHelper.Delete(Holiday))
            {
                ViewBag.message = "删除成功";
            }
            else
            {
                ViewBag.message = "删除不成功";
            }
            return View("index");
        }






    }
}
