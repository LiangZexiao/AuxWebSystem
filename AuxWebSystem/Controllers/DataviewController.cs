using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using AuxWebSystem.Models;
using AuxWebSystem.Helpers;
using AuxWebSystem.Filters;

namespace AuxWebSystem.Controllers
{

    [UserFilter(FailUrl = "/Home/Index", AdminRequire = false)]
    public class DataviewController : Controller
    {

        public ActionResult Index()
        {
            UserTableModel user = Session[UserSecurityHelper.sessionName] as UserTableModel;
            UserAreaModel userArea = new UserAreaModel(user.UserID);
            DataTable areas = DataBaseHelper.getRecordByKey(userArea);
            ViewBag.userAreas = areas;
            return View();
        }

        public void getStationByArea(String id)
        {
            Response.Write(JsonConvert.SerializeObject(new { area = id, stations = AreaStationHelper.getStationList(id) }));
        }



        //ok
        //1样机有效率分析统计：@MachineType NVARCHAR(200) 
        public void MachineUseEffectiveratio(String MachineType)
        {
            String sql = String.Format(@"EXEC MachineUseEffectiveRatio @MachineType= '{0}'", MachineType);
            DataTable dt = DataBaseHelper.getDataTableBySql(sql);
           // Response.Write(JsonConvert.SerializeObject(new { MachineType = MachineType, Value = sss++ }));
            Response.Write(JsonConvert.SerializeObject(new { MachineType = MachineType, Value = dt.Rows[0][0] }));
        }

        //Finsh
        //two options
        //2合格率分析@StationNO INT, @BeginTime DATETIME,@EndTme DATETIME
        [DataViewFilterAttribute(needSETime = true)]
        public void Passpercent(FormCollection Form)
        {
            String area = Form["Area"];
            String StationNumber = Form["StationNo"];
            String startTime = Form["StartTime"];
            String endTime = Form["EndTime"];
            DataRow[] rows = "-1".Equals(StationNumber)
                ? AreaStationHelper.getStation(area)
                : AreaStationHelper.getStation(area, StationNumber);

            List<DataViewJson> dataJson = new List<DataViewJson>();

            foreach (var row in rows)
            {
                String sql = String.Format(@"EXEC PassPercent @StationNO = '{0}', @BeginTime=N'{1}',@EndTime=N'{2}'", StationNumber, startTime, endTime);
                DataTable dt = DataBaseHelper.getDataTableBySql(sql);
                dataJson.Add(new DataViewJson(area, row["Area"].ToString(), StationNumber, row["StationName"].ToString(), dt.Rows[0][0]));
            }
            Response.Write(JsonConvert.SerializeObject(dataJson));
        }

        //3实验台有效利用率分析统计@StationNO INT,@BeginTime DATETIME,@EndTime DATETIME
        //two options
        [DataViewFilterAttribute(needSETime = true)]
        public void StationEffectiveUseratio(FormCollection Form)
        {
            String area = Form["Area"];
            String StationNumber = Form["StationNo"];
            String startTime = Form["StartTime"];
            String endTime = Form["EndTime"];

            DataRow[] rows = "-1".Equals(StationNumber)
              ? AreaStationHelper.getStation(area)
              : AreaStationHelper.getStation(area, StationNumber);

            List<DataViewJson> dataJson = new List<DataViewJson>();

            foreach (var row in rows)
            {
                String sql = String.Format(@"EXEC StationEffectiveUseRatio @StationNO = '{0}', @BeginTime=N'{1}',@EndTime=N'{2}'", StationNumber, startTime, endTime);
                DataTable dt = DataBaseHelper.getDataTableBySql(sql);
                dataJson.Add(new DataViewJson(area, row["Area"].ToString(), StationNumber, row["StationName"].ToString(), dt.Rows[0][0]));

            }
            Response.Write(JsonConvert.SerializeObject(dataJson));

        }

        //Finsh ok
        //4试验台电量统计@StationNO INT, @BeginTime DATETIME,@EndTime DATETIME
        //前台需要传入stationElectric，StationNo;StartTime，EndTime;（Area）
        [DataViewFilterAttribute(needSETime = true)]
        public void stationElectric(FormCollection Form)
        {
            String area = Form["Area"];
            String StationNumber = Form["StationNo"];
            String startTime = Form["StartTime"];
            String endTime = Form["EndTime"];

            DataRow[] rows = "-1".Equals(StationNumber)
                ? AreaStationHelper.getStation(area)
                : AreaStationHelper.getStation(area, StationNumber);

            List<DataViewJson> dataJson = new List<DataViewJson>();

            foreach (var row in rows)
            {
                String sql = String.Format(@"EXEC StationElectric @StationNO = '{0}', @BeginTime=N'{1}',@EndTime=N'{2}'", StationNumber, startTime, endTime);
                DataTable dt = DataBaseHelper.getDataTableBySql(sql);
                dataJson.Add(new DataViewJson(area, row["Area"].ToString(), StationNumber, row["StationName"].ToString(), dt.Rows[0][0]));
            }

            Response.Write(JsonConvert.SerializeObject(dataJson));
        }

        //5实验台利用率分析统计	@StationNO INT, @BeginTime DATETIME,@EndTime DATETIME
        //two options
        [DataViewFilterAttribute(needSETime = true)]
        public void StationUseratio(FormCollection Form)
        {
            String area = Form["Area"];
            String StationNumber = Form["StationNo"];
            String startTime = Form["StartTime"];
            String endTime = Form["EndTime"];
            DataRow[] rows = "-1".Equals(StationNumber)
                ? AreaStationHelper.getStation(area)
                : AreaStationHelper.getStation(area, StationNumber);

            List<DataViewJson> dataJson = new List<DataViewJson>();

            foreach (var row in rows)
            {
                String sql = String.Format(@"EXEC StationUseRatio @StationNO = '{0}', @BeginTime=N'{1}',@EndTime=N'{2}'", StationNumber, startTime, endTime);
                DataTable dt = DataBaseHelper.getDataTableBySql(sql);
                dataJson.Add(new DataViewJson(area, row["Area"].ToString(), StationNumber, row["StationName"].ToString(), dt.Rows[0][0]));
            }
            Response.Write(JsonConvert.SerializeObject(dataJson));

        }
        //ok
        //6委托单有效率分析统计@TestOrderNO NVARCHAR(200)
        public void TestOrderEffectiveUseratio(String TestOrderNO)
        {
            String sql = String.Format(@"EXEC TestOrderEffectiveUseRatio @TestOrderNO= '{0}'", TestOrderNO);
            DataTable dt = DataBaseHelper.getDataTableBySql(sql);
            Response.Write(JsonConvert.SerializeObject(new { TestOrderNO = TestOrderNO, Value = dt.Rows[0][0] }));
        }

        //Finsh
        //7委托单电量统计@TestOrder NVARCHAR(200)
        public void testOrderElectic(String TestOrderNO)
        {
            String sql = String.Format(@"EXEC TestOrderElectic @TestOrder= '{0}'", TestOrderNO);
            DataTable dt = DataBaseHelper.getDataTableBySql(sql);
            Response.Write(JsonConvert.SerializeObject(new { TestOrderNO = TestOrderNO, Value = dt.Rows[0][0] }));
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
