using Microsoft.Office.Interop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace AuxWebSystem.Helpers
{
    public class ExcelHelper
    {
        public String FileName { get; set; }

        /// <summary>
        /// ExcelFile\
        /// </summary>
        public static readonly String floderName = @"App_Data\";

        /// <summary>
        /// template.xlsx
        /// </summary>
        public static readonly String templateName = "template.xlsx";

        /// <summary>
        /// AppDomain.CurrentDomain.BaseDirectory + floderName
        /// </summary>
        public static readonly String FileDir = AppDomain.CurrentDomain.BaseDirectory + floderName;

        /// <summary>
        /// FileDir + @"\template.xlsx"
        /// </summary>
        public static readonly String TemplateFileDir = FileDir + @"\template.xlsx";

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
         * 注意：在遍历DataTable的时候，可是使用datatable.Rows[i]["Name"].ToString();Name为Name列的表头，
         *          所以Excel中列的顺序就无关紧要了。
         *          当然，前提是你知道Excel里列中各表头的名字。
         *          如果Excel中列的顺序固定，即可按下面代码中的方式进行。
         */

        /// <summary>
        /// 保存Excel文件
        /// </summary>
        public String SaveExcelFile()
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

            //如果文件存在，删除文件
            if (File.Exists( floderName + FileName))
            {
                File.Delete( floderName + FileName);
            }
            String filePAth = FileDir + FileName;
            request.Files[0].SaveAs(filePAth);
            Thread thread = new Thread(new ParameterizedThreadStart(DoDeleteFile));
            thread.Start(filePAth);
            return FileName;
        }

        /// <summary>
        /// 用户下载文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        //public static HttpResponseMessage UserDownloadFile(String fileName)
        //{
        //    String filePath = FileDir + fileName;
        //    FileStream fstream = new FileStream( filePath, System.IO.FileMode.Open);
        //    HttpResponseMessage resp = new HttpResponseMessage();
        //    resp.Content = new StreamContent(fstream);
        //    //MimeMapping.GetMimeMapping(fileName);
        //    resp.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    //response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
        //    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = fileName
        //    };
        //    return resp;
        //}
        public void UserDownloadFile(String fileName)
        {
            string filePath = FileDir + fileName;//路径
            FileInfo fileInfo = new FileInfo(filePath);
            HttpResponse Response = HttpContext.Current.Response;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());//告诉浏览器是下载文件，而不是打开文件
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = MimeMapping.GetMimeMapping(fileName);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();

            Thread thread = new Thread(new ParameterizedThreadStart(DoDeleteFile));
            thread.Start(filePath);
        }

        /// <summary>
        /// 从Excel中导入
        /// </summary>
        /// <param name="fileName">Excel文件名字</param>
        /// <returns></returns>
        public DataTable getExcelData(String fileName)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = false;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = excelApp.Workbooks;
            //这里的Add方法里的参数就是模板的路径
            //template.xlsx
            //Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(TemplateFileDir);

            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

            /*
             * 	
                Hi I found a very much faster way.

                It is better to read the entire data in one go using "get_range". This loads the data into memory and I can loop through that like a normal array.

                Microsoft.Office.Interop.Excel.Range range = gXlWs.get_Range("A1", "F188000");
                object[,] values = (object[,])range.Value2;
                int NumRow=1;
                while (NumRow < values.GetLength(0))
                {
                    for (int c = 1; c <= NumCols; c++)
                    {
                        Fields[c - 1] = Convert.ToString(values[NumRow, c]);
                    }
                    NumRow++;
                }
             */
            //Microsoft.Office.Interop.Excel.Range range = excelApp.Range
            return null;
        }


        /// <summary>
        /// 把User表的数据导出成Excel文件
        /// </summary>
        /// <param name="datatable">DataTable</param>
        /// <returns>导出文件的物理地址</returns>
        public String ExportExcel(DataTable datatable)
        {
            if (null == datatable || datatable.Rows.Count == 0)
            {
                throw new ExcelHelperException("数据为空");
            }

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            if (null == excelApp)
            {
                //对此实例进行验证，如果为null则表示运行此代码的机器可能未安装Excel
                throw new ExcelHelperException("Excel无法打开");
            }

            //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写 
            excelApp.Visible = false;

            //保存文化环境
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Microsoft.Office.Interop.Excel.Workbooks workbooks = excelApp.Workbooks;
            //这里的Add方法里的参数就是模板的路径
            //template.xlsx
            //Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(TemplateFileDir);

            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

            Microsoft.Office.Interop.Excel.Range range;


            //for (int i = 1; i <= datatable.Columns.Count; i++)
            //{
            //    worksheet.Cells[1, i] = datatable.Columns[i-1].ColumnName;
            //    //range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i];
            //    //range.Interior.ColorIndex = 15;
            //    //range.Font.Bold = true;
            //}

            int columnCount = datatable.Columns.Count;
            int rowCount = datatable.Rows.Count;

            range = excelApp.Range[excelApp.Cells[2, 1], excelApp.Cells[rowCount + 1, columnCount]];
            //range = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[rowCount + 1, columnCount]);

            //创建对象数组存储DataTable的数据，这样的效率比直接将Datateble的数据填充worksheet.Cells[row,col]高
            Object[,] dat = new Object[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    //dat[i, j] = datatable.Rows[i][j] == null ? "" : datatable.Rows[i][j].ToString();
                    dat[i, j] = datatable.Rows[i][j];
                }

            }
            range.Value2 = dat;

            //恢复文化环境
            System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

            string fileName = DateTime.Now.Ticks.ToString() + ".xlsx";
            String savePath = FileDir + fileName;
            workbook.SaveCopyAs(savePath);
            workbook.Saved = true;

            workbook.Close();

            excelApp.Quit();

            return fileName;
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

        /// <summary>
        /// 用于删除文件
        /// 注意：要使用其他线程运行
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private void DoDeleteFile(Object filePath)
        {
            Thread.Sleep(TimeSpan.FromMinutes(30));
            String file = filePath as String;
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }




    }

    public class ExcelHelperException : Exception
    {
        public ExcelHelperException(String message)
            : base(message)
        {

        }
    }

}