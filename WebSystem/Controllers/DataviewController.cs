using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebSystem.Models;
using WebSystem.Helpers;

namespace WebSystem.Controllers
{
    
    public class DataviewController : Controller
    {
        //
        // GET: /Dataview/
        string str = @"数据库名字";
        public ActionResult Index(FormCollection form)
        {
            
            //数据查看
            SqlConnection conn =new SqlConnection(str);
            List<DataviewModel> list = new List<DataviewModel>();
            conn.Open();
            DataviewModel Views=new DataviewModel();
            Views.Area = form["Area"];
            Views.Station = form["Station"];
            Views.Datatime = DateTime.Parse( form["Datetime"] );          
            string sql = "selcet Area,Station,Datetiome from table where area= '"+Views.Area+"',"+"and Station = '"+Views.Station+"',"+" and Datatime= '"+Views.Datatime+"'";
            SqlCommand comm=new SqlCommand(sql,conn);
            SqlDataReader reader=comm.ExecuteReader();
            if(!reader.Read())
            {
                reader.Close();
                throw new UserSecurityException("该区域，时间内，没有样机工作(T_T)");

            }
            else
            {
                DataviewModel tmp = new DataviewModel();
                do
                {
                    Views.Starttime = reader.GetDateTime(4);
                    Views.Endtime = reader.GetDateTime(5);
                    tmp.Starttime = reader.GetDateTime(4);
                    tmp.Endtime = reader.GetDateTime(5);
                    tmp.efficient = DateviewHelper.Runtime(tmp);
                    list.Add(tmp);
                    

                }
                while (reader.Read());
            }
            reader.Close();
            conn.Close();
            List<double> Efficient = new List<double>();
            ViewBag.e = Efficient;
        

            return View();
            }
        }

    }

