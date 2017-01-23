using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Newtonsoft.Json;
using AuxWebSystem.Helpers;
using AuxWebSystem.Models;

namespace AuxWebSystem.Controllers
{

    public class LabInfoController : Controller
    {
        //
        // GET: /LabInfo/
        public ActionResult Index()
        {
            DataTable LabInfo;
            if (null == HttpRuntime.Cache[LabInfoModel.cacheName])
            {
                LabInfo = DataBaseHelper.getAllRecord(new LabInfoModel());
                HttpRuntime.Cache[LabInfoModel.cacheName] = LabInfo;
            }
            else
            {
                LabInfo = HttpRuntime.Cache[LabInfoModel.cacheName] as DataTable;
            }
            ViewBag.Area = SystemParameterHelpers.getInstance().Select("Area");
            ViewBag.LabInfo = LabInfo;
            return View();
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
            model.Area = helper.Select("Area", Value);
            if (DataBaseHelper.hasMyKeyRecord(model))      //寻找是否有相同灯号的灯
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 0, message = "灯号或者灯名不能重复" }));
                return;
            }
            else
            {
                if (DataBaseHelper.Insert(model))
                {
                    Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "插入成功" }));
                }
                else
                {
                    Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "插入失败" }));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DelectLabInFo(int id)
        {
            LabInfoModel LabInFo = new LabInfoModel();
            LabInFo.StationNO = id;
            if (DataBaseHelper.Delete(LabInFo))
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "删除成功" }));
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { state = 1, message = "删除失败" }));
            }
        }

    }
}
