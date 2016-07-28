using Loyung.DBModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Loyung.PublicMethod
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            this.DBHelper = new LoyungDBDataContext();
        }
        /// <summary>
        /// 数据上下文操作实例
        /// </summary>
        protected LoyungDBDataContext DBHelper;

        /// <summary>
        /// 系统当前登录用户
        /// </summary>
        public Userinfo Userinfo
        {
            get { return Userinfo; }
            set
            {
                if (Session["Userinfo"] != null)
                {
                    Userinfo = (Userinfo)Session["Userinfo"];
                }
            }
        }

        /// <summary>
        ///  API中使用:返回JSON序列化数据,以及状态消息
        /// </summary>
        /// <param name="ErrMsg">序列化的错误消息</param>
        /// <param name="Data">序列化数据</param>
        /// <returns>序列化json</returns>
        public JsonResult ReturnJsonApi(string ErrMsg, object Data)
        {
            var obj = new
            {
                errMsg = ErrMsg ?? "",
                data = Data ?? "",
                resultStatus = string.IsNullOrWhiteSpace(ErrMsg) ? 0 : 1,
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// API中使用:返回JSON序列化数据,以及状态消息
        /// </summary>
        /// <param name="Data">返回的正式数据</param>
        /// <returns>序列化json</returns>
        public JsonResult ReturnJsonApi(object Data)
        {
            return ReturnJsonApi(null, Data);
        }

        /// <summary>
        /// 直接返回json序列化后数据
        /// </summary>
        /// <param name="Data">需要json序列化的数据</param>
        /// <returns>返回json序列化的数据</returns>
        public JsonResult ReturnJson(object Data)
        {
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

    }
}
