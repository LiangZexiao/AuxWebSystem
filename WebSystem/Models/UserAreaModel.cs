using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSystem.Models
{
    /*					
     * 表中文名称	        用户区域表				
     * 表英文名称	        UserArea				
     * 表用途	            查询用户区域				
     * 所在位置	            服务器				
     * 主键	                User,UserArea				
     * 
     * 中文名称     英文字段	    类型	    是否可为空	外键关系	    备注
     * 用户名	    UserID	    int	    否	        User	
     * 区域	        Area	    int	    否		
     */
    public class UserAreaModel : TableModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        /// 区域
        /// </summary>
        public int Area { set; get; }

        public UserAreaModel()
        {
            TableName = @"UserArea";
        }


    }
}