using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Newtonsoft.Json;
using AuxWebSystem.Helpers;
using AuxWebSystem.Models;
using AuxWebSystem.Filters;

namespace AuxWebSystem.Controllers
{
    [UserFilter(FailUrl = "/Home/Index", AdminRequire = true)]
    public class LabInfoController : Controller
    {
        //
        // GET: /LabInfo/
        public ActionResult Index()
        {
            ViewBag.Area = SystemParameterHelpers.getInstance().Select("Area");
            ViewBag.LabInfo = AreaStationHelper.getAreaStationDatatable();
            return View();
        }

        /// <summary>
        /// 用于检查灯号
        /// </summary>
        /// <param name="id"></param>
        public void checkLampNO(int id)
        {
            DataTable dt = AreaStationHelper.getAreaStationDatatable();
            if (null == dt)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "发生错误, 请刷新后" }));
                return;
            }

            DataRow[] rows = dt.Select(String.Format("LampNO = '{0}'", id));
            if (rows.Length > 0)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "灯号重复" }));
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "成功" }));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        public void AddLabInFo(FormCollection form)
        {
            LabInfoModel model = new LabInfoModel();
            int lampno;
            if (int.TryParse(form["LampNO"], out lampno))
            {
                model.LampNO = lampno;
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "灯号为空" }));
                return;
            }

            model.StationName = form["StationName"];
            model.Propety = form["Propety"];
            model.LampText = form["LampText"];
            string Value = form["Area"];

            
            SystemParameterHelpers helper = SystemParameterHelpers.getInstance();
            //model.Area = helper.Select("Area", Value);

            if (null == Value)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "区域为空" }));
                return;
            }

            try
            {
                model.Area = Convert.ToInt32(Value);
            }
            catch (Exception e)
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "请选择区域" }));
                return;
            }
            

            if (DataBaseHelper.hasMyRecord(model))      //寻找是否有相同灯号的灯
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "灯号或者灯名不能重复" }));
                return;
            }
            else
            {
                if (DataBaseHelper.Insert(model))
                {
                    DataBaseHelper.fillOneRecordToModel(model);
                    Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "插入成功", StationNO = model.StationNO }));
                    AreaStationHelper.removeAreaStationTable();
                }
                else
                {
                    Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "插入失败" }));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">StationNO</param>
        public void DelectLabInFo(int id)
        {
            LabInfoModel LabInFo = new LabInfoModel(id);
            if (DataBaseHelper.Delete(LabInFo))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "删除成功" }));
                AreaStationHelper.removeAreaStationTable();
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "删除失败" }));
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
