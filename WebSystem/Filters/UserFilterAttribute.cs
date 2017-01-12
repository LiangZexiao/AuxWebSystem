using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using WebSystem.Models;
using WebSystem.Helpers;

namespace WebSystem.Filters
{
    /*
     * 授权筛选器：AuthorizationFilters
     * 动作筛选器：ActionFilters
     * 响应筛选器：ResultFilters
     * 异常筛选器：ExceptionFilters
     * 
     * Controller最终是通过Controller的ExecuteCore完成的，
     * 这个方法通过调用ControllerActionInvoker的InvodeAction方法完成最终对于Action的调用。
     */
    public class UserFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 错误UEL
        /// </summary>
        public String FailUrl { set; get; }

        /// <summary>
        /// 是否需要管理员权限
        /// </summary>
        public bool AdminRequire { set; get; }

        /// <summary>
        /// 请求授权时执行
        /// 在Action执行之前由 MVC 框架调用
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionState session = HttpContext.Current.Session;
            Object obj = session[UserSecurityHelper.sessionName];
            if (null == obj)
            {
                filterContext.Result = new RedirectResult(FailUrl);
            }
            else
            {
                UserTableModel model = (UserTableModel)obj;
                if (AdminRequire && !model.isAdminUser() )
                {
                    filterContext.Result = new RedirectResult(FailUrl);
                }

            }
            
        }



    }
}
