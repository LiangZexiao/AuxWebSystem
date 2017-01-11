using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace WebSystem.Helpers
{
    public class ExcelHelper
    {
        public String FileName { get; set; }

        public ExcelHelper()
        {

        }

        /*
         * 实现的基本思想：
         * 1，先使用FileUpload控件fuload将Excel文件上传到服务器上得某一个文件夹。
         * 2，使用OleDb将已经上传到服务器上的Excel文件读出来，
         *      这里将Excel文件当做一个数据库来读。在联系数据库语句中
         *      Data Source就是该文件在服务器上得物理路径
         * 3，将第二步中读出的数据以DataTable对象返回。
         * 4，遍历DataTable对象，然后到Sql Server数据库中查询，是否存在该条数据。
         *      如果存在，可以做更新，或者不做处理；
         *      如果不存在，则插入数据。
         * 
         * 注意：在遍历DataTable的时候，可是使用dt.Rows[i]["Name"].ToString();Name为Name列的表头，
         *          所以Excel中列的顺序就无关紧要了。
         *          当然，前提是你知道Excel里列中各表头的名字。
         *          如果Excel中列的顺序固定，即可按下面代码中的方式进行。
         */

        public void SaveFile()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Files.Count <= 0)
            {
                throw new ExcelHelperException("no file upload");
            }
            FileName = request.Files[0].FileName;
            if (null == FileName || !Regex.IsMatch(FileName, @"(.+)\.((xlsx)|(xls))", RegexOptions.IgnoreCase))
            {
                throw new ExcelHelperException("文件格式不符合");
            }

            String fileDir = "ExcelFile/" + FileName;

            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }

            
            //如果文件存在，删除文件
            if (File.Exists(HttpContext.Current.Server.MapPath(fileDir)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(fileDir));
            }

            request.Files[0].SaveAs(fileDir);

        }

        public DataTable ExcelData()
        {

            return null;

        }


        /*
         * 使用DataGird生成Excel
         * 
         * 基本思想：
         * 1. 将数据从数据库中查询出来，绑定到DataGrid控件中，
         *      这个DataGirdle控件知识作为数据的一个承载，不需要显示在页面中
         * 2. 使用StringWriter将DataGrid读出来，在使用Response的另存为功能，
         *      将html页存为Xls格式的Excel文件。
         */


    }

    public class ExcelHelperException : Exception
    {
        public ExcelHelperException(String message)
            : base(message)
        {

        }
    }

}