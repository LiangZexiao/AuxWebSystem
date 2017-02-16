using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using AuxWebSystem.Models;
using AuxWebSystem.Helpers;
using AuxWebSystem.Filters;

namespace AuxWebSystem.Controllers
{
    [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
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
            /*参数类型	    ParameterType
             *              ChineseName
             *参数编号	    ParameterNO
             *参数值	        Value
             *是否可修改	    Revisable
             */
            SystemParameterHelpers sysHelper = SystemParameterHelpers.getInstance();
            ViewBag.Parameter = sysHelper.getParameterTypeDictionary();
            ViewBag.SystemTable = sysHelper.getDataTable();
            return View();
        }

        /// <summary>
        /// 添加系统参数
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public void SystemParameterAdding(FormCollection form)
        {
            /*
             * 参数类型	    ParameterType	nvarchar(50)
             * 参数编号	    ParameterNO	    int
             * 参数值	    Value	        char(100)
             * 是否可修改	Revisable	    int
             */
            String parameterType = form["ParameterType"];
            String value = form["Value"];
            try
            {
                SystemParameterHelpers helper = SystemParameterHelpers.getInstance();
                int experimentNo = helper.Add(parameterType, value);
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功", ExperimentNo = experimentNo }));
            }
            catch (Exception e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = e.Message, ExperimentNo = -1 }));
            }
        }

        /// <summary>
        /// 删除系统参数
        /// </summary>
        /// <param name="id">参数编号 ParameterNO</param>
        /// <param name="value">参数类型 ParameterType</param>
        /// <returns></returns>
        public void SystemParameterDelete(int id, String value)
        {
            SystemParameterHelpers helper = SystemParameterHelpers.getInstance();
            try
            {
                helper.Delect(value, id);
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
            }
            catch (Exception e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = e.Message }));
            }
        }

        /// <summary>
        /// 标准实验
        /// </summary>
        /// <returns></returns>
        public ActionResult StandardParameter()
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
        public void StandardExperimentAdd(FormCollection Form)
        {
            StandardExperimentModels TimeParameter = new StandardExperimentModels();
            TimeParameter.ExperimentName = Form["ExperimentName"];
            TimeParameter.ExperimentNo = Form["ExperimentNo"];

            if ("".Equals(TimeParameter.ExperimentName) || "".Equals(TimeParameter.ExperimentNo))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "实验名称或实验类型编号为空" }));
                return;
            }

            if (DataBaseHelper.hasMyKeyRecord(TimeParameter))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "试验类型编号重复" }));
                return;
            }
            float tempfloat;

            if ( float.TryParse(Form["StandardTestHours"], out tempfloat))
            {
                TimeParameter.StandardTestHours = tempfloat;
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "标准时间有误" }));
                return;
            }

            if (DataBaseHelper.Insert(TimeParameter))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "增加不成功" }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ExperimentNo</param>
        public void StandardExperimentDelect(string id)
        {
            StandardExperimentModels SE = new StandardExperimentModels();
            SE.ExperimentNo = id;
            if (DataBaseHelper.Delete(SE))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "删除成功" }));
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "删除不成功" }));
            }
        }

        public ActionResult Holiday()
        {
            DateTime nowdate = DateTime.Now;
            int mouth = nowdate.Month;
            int year = nowdate.Year;
            return getHolidayByMouthAndYear(mouth, year);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">月</param>
        /// <param name="value">年</param>
        /// <returns></returns>
        public ActionResult getHolidayByMouthAndYear(int id, int value)
        {
            int year = value;
            int mouth = id;
            HolidayModels model = new HolidayModels();
            model.Time = mouth > 9 ? year + "-" + mouth : year + "-0" + mouth;
            DataTable mouthTable = DataBaseHelper.getLikeRecord(model);

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //StartTime,EndTime
            DateTime startTime, endTime;
            foreach (DataRow row in mouthTable.Rows)
            {
                startTime = Convert.ToDateTime(row["StartTime"]);
                endTime = Convert.ToDateTime(row["EndTime"]);
                while (startTime.Day <= endTime.Day)
                {
                    if (mouth == startTime.Month)
                    {
                        builder.AppendFormat("\"{0}\", ", startTime.ToString("MM-dd-yyyy"));
                    }
                    startTime = startTime.AddDays(1);
                }
            }
            if (builder.Length > 4)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            ViewBag.displayDate = year+"-" + mouth + "-1";
            ViewBag.Mouth = mouthTable;
            ViewBag.dayString = builder.ToString();
            return View("Holiday");
        }

        public void AddHoliday(FormCollection Form)
        {
            HolidayModels Holiday = new HolidayModels();
            String startTime = Form["StartTime"];
            String endTime = Form["EndTime"];

            if (String.IsNullOrWhiteSpace(startTime) || String.IsNullOrWhiteSpace(endTime))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "请检查放假时间" }));
                return;
            }

            Holiday.StartTime = Convert.ToDateTime(startTime);
            Holiday.EndTime = Convert.ToDateTime(endTime);

            Holiday.HolidayReason = Form["HolidayReason"];

            //DataTable datatable = DataBaseHelper.getAllRecord(Holiday);
            if (!DataBaseHelper.hasMyKeyRecord(Holiday))
            {
                if (DataBaseHelper.Insert(Holiday))
                {
                    DataBaseHelper.fillOneRecordToModel(Holiday);
                    Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "添加成功", holidayID = Holiday.ID }));
                    return;
                }
                else
                {
                    Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "添加不成功" }));
                }
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "放假时间有重叠" }));
            }
        }

        /// <summary>
        /// 删除节假日
        /// </summary>
        /// <param name="id"></param>
        public void DeleteHoliday(int id)
        {
            HolidayModels Holiday = new HolidayModels();
            Holiday.ID = id;
            if (DataBaseHelper.Delete(Holiday))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "删除成功" }));
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "删除不成功" }));
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // 标记异常已处理
            filterContext.ExceptionHandled = true;
            // 跳转到错误页
            filterContext.Result = new HttpStatusCodeResult(404);
        }

    }
}
