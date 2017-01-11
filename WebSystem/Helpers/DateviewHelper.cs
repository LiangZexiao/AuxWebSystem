using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSystem.Models;


namespace WebSystem.Helpers
{
    public class DateviewHelper
    {
        public static double Runtime(DataviewModel views)
        {
            TimeSpan runtime = views.Endtime - views.Starttime;
            double getHour = runtime.TotalHours;
            return getHour;
        }
    }
}