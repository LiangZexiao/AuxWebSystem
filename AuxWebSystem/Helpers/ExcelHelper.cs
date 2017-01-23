using NPOI;
using NPOI.XSSF;
using NPOI.XSSF.Extractor;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Util;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.Extractor;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.SS.UserModel;
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
        public static readonly String TemplateFileDir = FileDir + @"template.xlsx";

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
        /// <summary>
        /// 用户下载文件
        /// </summary>
        /// <param name="fileName">Excel文件的名字</param>
        /// <param name="downLoadFileName"></param>
        public void UserDownloadFile(String fileName, String downLoadFileName)
        {
            string filePath = FileDir + fileName;//路径
            FileInfo fileInfo = new FileInfo(filePath);
            HttpResponse Response = HttpContext.Current.Response;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(downLoadFileName, System.Text.Encoding.UTF8));
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());//告诉浏览器是下载文件，而不是打开文件
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = MimeMapping.GetMimeMapping(fileName);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();
        }

        /// <summary>
        /// 从Excel中导入获得DataTable
        /// </summary>
        /// <param name="fileName">Excel文件名字</param>
        /// <returns>Datatable</returns>
        public DataTable getExcelData(String fileName)
        {
            /*
             * [User]                   
             * [UserID]             [SystemParameter]
             * [LogName]       
             * [RealName]           [ParameterType] 
             * [Password]           [ParameterNO]   
             * [Department]         [Value]         
             * [UserType]           [Revisable]     
             * [Email]         
             * [CellPhone]     
             * [LastLoginTime] 
             */
            //登录名	真实姓名	部门	用户类型	邮箱	电话号码
            FileStream fstream = File.Open(FileDir + fileName, FileMode.Open);
            IWorkbook workBook = null;
            if (Regex.IsMatch(fileName, @"(.+)\.(xlsx)", RegexOptions.IgnoreCase))
            {
                workBook = new XSSFWorkbook(fstream);
            }
            else if (Regex.IsMatch(fileName, @"(.+)\.(xls)", RegexOptions.IgnoreCase))
            {
                workBook = new HSSFWorkbook(fstream);
            }
            else
            {
                throw new ExcelHelperException("文件格式不符合");
            }

            ISheet sheet = workBook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);//读取当前行数据

            //LastRowNum 是当前表的总行数-1（注意）
            DataTable datatable = new DataTable();
            //datatable.Columns.Add("UserID");
            datatable.Columns.Add("LogName");
            datatable.Columns.Add("RealName");
            datatable.Columns.Add("Department");
            datatable.Columns.Add("UserType");
            datatable.Columns.Add("Email");
            datatable.Columns.Add("CellPhone");
            datatable.Columns.Add("LastLoginTime");
            DataRow dataRow;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                row = sheet.GetRow(i);
                if (null != row)
                {
                    //LastCellNum 是当前行的总列数
                    dataRow = datatable.NewRow();
                    dataRow["LogName"] = row.GetCell(0);
                    dataRow["RealName"] = row.GetCell(1);
                    dataRow["Department"] = getDepartment(row.GetCell(2));
                    dataRow["UserType"] = getUserType(row.GetCell(3));
                    dataRow["Email"] = row.GetCell(4);
                    dataRow["CellPhone"] = row.GetCell(5);
                    datatable.Rows.Add(dataRow);
                }
            }

            fstream.Close();

            return datatable;

            /*
             * 		//读取当前表数据
		ISheet sheet = wk.GetSheetAt(0);

		IRow row = sheet.GetRow(0);  //读取当前行数据
		//LastRowNum 是当前表的总行数-1（注意）
		int offset = 0;
		for (int i = 0; i <= sheet.LastRowNum; i++)
		{
			row = sheet.GetRow(i);  //读取当前行数据
			if (row != null)
			{
				//LastCellNum 是当前行的总列数
				for (int j = 0; j < row.LastCellNum; j++)
				{
					//读取该行的第j列数据
					string value = row.GetCell(j).ToString();
					Console.Write(value.ToString() + " ");
				}
				Console.WriteLine("\n");
			}
		}
	}
             */

        }

        /// <summary>
        /// 把Datatable添加到数据库
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool addDataTableToDatabase(DataTable table)
        {
            //声明一个事务对象
            SqlTransaction tran = null;
            try
            {
                SqlConnection conn = DataBaseHelper.getSqlConnection();
                conn.Open();
                tran = conn.BeginTransaction();
                SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran);
                copy.ColumnMappings.Add("LogName", "LogName");
                copy.ColumnMappings.Add("RealName", "RealName");
                copy.ColumnMappings.Add("Department", "Department");
                copy.ColumnMappings.Add("UserType", "UserType");
                copy.ColumnMappings.Add("Email", "Email");
                copy.ColumnMappings.Add("CellPhone", "CellPhone");

                copy.DestinationTableName = "[User]";
                copy.WriteToServer(table);
                tran.Commit();
                copy.Close();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                if (null != tran)
                {
                    tran.Rollback();
                }
                throw e;
            }
        }

        /// <summary>
        /// 获得用户类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private dynamic getUserType(dynamic value)
        {
            if (null == value)
            {
                return null;
            }
            return SystemParameterHelpers.getInstance().Select("UserType", value.ToString());
        }

        /// <summary>
        /// 获得部门
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private dynamic getDepartment(dynamic value)
        {
            if (null == value)
            {
                return null;
            }
            return SystemParameterHelpers.getInstance().Select("Department", value.ToString());
        }

        /// <summary>
        /// 得到User表
        /// </summary>
        /// <returns></returns>
        public DataTable getUserTable()
        {
            //登录名	真实姓名	部门	用户类型	邮箱	电话号码
            /*
             * [User]                   
             * [UserID]             [SystemParameter]
             * [LogName]       
             * [RealName]           [ParameterType] 
             * [Password]           [ParameterNO]   
             * [Department]         [Value]         
             * [UserType]           [Revisable]     
             * [Email]         
             * [CellPhone]     
             * [LastLoginTime] 
             */

            String sql = @"SELECT [User].LogName, [User].RealName, TempDepartment.Value AS Department, TempUserType.Value AS UserType, [User].Email, [User].CellPhone
                            FROM [User]
                            LEFT JOIN SystemParameter AS TempDepartment
                            ON 'Department' = TempDepartment.ParameterType AND TempDepartment.ParameterNO = [User].Department
                            LEFT JOIN SystemParameter AS TempUserType
                            ON 'UserType' = TempUserType.ParameterType AND TempUserType.ParameterNO = [User].UserType";
            return DataBaseHelper.getDataTableBySql(sql);
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

            String fileName = DateTime.Now.Ticks.ToString() + ".xlsx";
            String filePath = FileDir + fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            //File.Create(fileName);
            File.Copy(TemplateFileDir, filePath);

            FileStream fstream = File.Open(filePath, FileMode.Open);
            
            IWorkbook workbook = new XSSFWorkbook(fstream);
            //创建工作薄 
            ICellStyle style = workbook.CreateCellStyle();//样式

            int columnCount = 6;

            ISheet sheet = workbook.GetSheetAt(0);
            IRow row;
            ICell cell;
            int rowCounter = 1;
            foreach (DataRow dataRow in datatable.Rows)
            {
                row = sheet.CreateRow(rowCounter);
                rowCounter += 1;
                for (int i = 0; i < columnCount; i++)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(dataRow[i] == null ? "" : dataRow[i].ToString());
                }
            }

            fstream.Close();
            fstream = File.OpenWrite(filePath);
            workbook.Write(fstream);
            fstream.Close();

            Thread thread = new Thread(new ParameterizedThreadStart(DoDeleteFile));
            thread.Start(FileDir + fileName);

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
            Thread.Sleep(TimeSpan.FromMinutes(1));
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