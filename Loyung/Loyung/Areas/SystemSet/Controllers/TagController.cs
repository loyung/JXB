using Loyung.DBModels;
using Loyung.PublicMethod;
/*
 * 创建时间：2016-06-03
 * 创建人：刘自洋
 * 说明：标签管理控制器，主要为人源添加标签
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loyung.Areas.SystemSet.Controllers
{
    /// <summary>
    /// 标签管理控制器
    /// </summary>
    public class TagController : BaseController
    {
        #region 页面加载

        public ActionResult TagMain()
        {
            return View();
        }

        public ActionResult TagLeft()
        {
            return View();
        }

        public ActionResult TagRight(int? id)
        {
            return View();
        }

        #endregion

        #region 树状页
        public ActionResult GetTagList()
        {
            var systas = DBHelper.SYSTA.Where(ta=>ta.TA003.Equals(-1));
            if (systas != null)
            {
                var TreeList = systas.Select(ta => new { id = ta.ID, text = ta.TA001, children = DBHelper.SYSTA.Where(t => t.TA003.Equals(ta.ID)).Select(tt => new { id = tt.ID, text = tt.TA001 }) });
                return ReturnJson(TreeList);
            }
            else
            { return null; }
        }
        #endregion

        #region 内容页

        /// <summary>
        /// 返回单项综合数据
        /// </summary>
        /// <param name="id">单项元素ID</param>
        /// <returns></returns>
        public ActionResult BindInfo(int id)
        {
            SYSTA systa = DBHelper.SYSTA.FirstOrDefault(ta => ta.ID.Equals(id));
            if (systa != null)
            {
                return ReturnJson(new
                {
                    ID = systa.ID,
                    TA001 = systa.TA001,
                    TA002 = systa.TA002,
                    TA003 = systa.TA003,
                    TA004 = systa.TA004,
                });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取所有模块，绑定下拉列表
        /// </summary>
        /// <returns>value:模块编号,text:模块名称</returns>
        public ActionResult GetAllTag()
        {
            var systas = DBHelper.SYSTA.OrderBy(ta => ta.TA002);
            if (systas != null)
            {
                return ReturnJson(systas.Select(se => new { value = se.ID, text = se.TA001 }));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存新增(修改)模型
        /// </summary>
        /// <returns>是否添加(修改)模块成功</returns>
        public ActionResult AddInfo()
        {
            try
            {
                SYSTA systa = null;
                if (Request.Form["info_id"] == "#")
                {
                    systa = DBHelper.SYSTA.FirstOrDefault(ta => ta.TA003.Equals(int.Parse(Request.Form["TA003"])) && ta.TA001.Equals(Request.Form["TA001"]));
                    if (systa == null)
                    {
                        systa = new SYSTA();
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    systa = DBHelper.SYSTA.FirstOrDefault(ta => ta.ID.Equals(Request.Form["info_id"]));
                }
                systa.TA001 = Request.Form["TA001"];
                systa.TA002 = Request.Form["TA002"];
                systa.TA003 = int.Parse(Request.Form["TA003"]);
                systa.TA004 = Request.Form["TA004"];
                if (Request.Form["info_id"] == "#")
                    DBHelper.SYSTA.InsertOnSubmit(systa);
                DBHelper.SubmitChanges();
                return Content("true");

            }
            catch
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="info_id"></param>
        /// <returns></returns>
        public ActionResult DeleteInfo(int info_id)
        {
            SYSTA systa = DBHelper.SYSTA.FirstOrDefault(ta => ta.ID.Equals(info_id));
            if (systa != null)
            {
                DBHelper.SYSTA.DeleteOnSubmit(systa);
                DBHelper.SubmitChanges();
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }

        #endregion

    }
}