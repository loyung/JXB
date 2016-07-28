/*
 * 创建时间：2016-05-23
 * 创建人：刘自洋
 * 说明：菜单管理jstree，目前支持两级菜单
 */
using Loyung.DBModels;
using Loyung.PublicMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Loyung.Areas.SystemSet.Controllers
{
    /// <summary>
    /// 菜单设置
    /// </summary>
    public class MenuController : BaseController
    {
        public ActionResult MenuMain()
        {
            return View();
        }

        public ActionResult MenuLeft()
        {
            return View();
        }

        public ActionResult MenuRight(int? id)
        {
            return View();
        }

        /// <summary>
        /// 返回单项综合数据
        /// </summary>
        /// <param name="id">单项元素ID</param>
        /// <returns></returns>
        public ActionResult BindInfo(int id)
        {
            SYSMA sysma = DBHelper.SYSMA.FirstOrDefault(ma => ma.ID.Equals(id));
            if (sysma != null)
            {
                return ReturnJson(new
                {
                    ID = sysma.ID,
                    MA001 = sysma.MA001,
                    MA002 = sysma.MA002,
                    MA003 = sysma.MA003,
                    MA004 = sysma.MA004,
                    MA005 = sysma.MA005,
                    MA006 = sysma.MA006,
                    MA007 = sysma.MA007
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
        public ActionResult GetAllModel()
        {
            var sysmas = DBHelper.SYSMA.Where(ma => ma.MA007.Equals(true)).OrderBy(or => or.MA004);
            if (sysmas != null)
            {
                return ReturnJson(sysmas.Select(se => new { value = se.ID, text = se.MA001 }));
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
                SYSMA sysma = null;
                if (Request.Form["info_id"] == "#")
                {
                    sysma = DBHelper.SYSMA.FirstOrDefault(ma => ma.MA005.Equals(int.Parse(Request.Form["model_pid"])) && ma.MA001.Equals(Request.Form["model_name"]));
                    if (sysma == null)
                    {
                        sysma = new SYSMA();
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    sysma = DBHelper.SYSMA.FirstOrDefault(ma => ma.ID.Equals(Request.Form["info_id"]));
                }
                sysma.MA001 = Request.Form["model_name"];
                sysma.MA002 = Request.Form["model_ico"];
                sysma.MA003 = Request.Form["model_url"];
                sysma.MA004 = Request.Form["model_order"];
                sysma.MA005 = int.Parse(Request.Form["model_pid"]);
                sysma.MA006 = Request.Form["model_mark"];
                sysma.MA007 = true;
                if (Request.Form["info_id"] == "#")
                    DBHelper.SYSMA.InsertOnSubmit(sysma);
                DBHelper.SubmitChanges();
                //子模块默认没有权限点时创建查看权限点
                var LimitCount = DBHelper.SYSMB.Where(mb => mb.MB001.Equals(sysma.ID));
                if (sysma.MA005.Value != -1 && LimitCount != null && LimitCount.Count() == 0)
                {
                        SYSMB newsysmb = new SYSMB();
                        newsysmb.MB001 = sysma.ID;
                        newsysmb.MB002 = sysma.MA001 + "查看";
                        newsysmb.MB003 = DateTime.Now.ToString("yyyyMMddHHmmss") + "_View";
                        newsysmb.MB004 = true;
                        DBHelper.SYSMB.InsertOnSubmit(newsysmb);
                        DBHelper.SubmitChanges();
                }
                return Content("true");

            }
            catch
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 检测模型名字当前模块下是否存在
        /// </summary>
        /// <returns>truee可以添加当前模块|false当前模块已存在</returns>
        public ActionResult CheckModelName()
        {
            if (!string.IsNullOrWhiteSpace(Request["model_name"]) && !string.IsNullOrWhiteSpace(Request["pid"]))
            {
                string Pid = Request["pid"];
                string model_name = Request["model_name"];
                SYSMA sysma = DBHelper.SYSMA.FirstOrDefault(ma => ma.MA005.Equals(int.Parse(Pid)) && ma.MA001.Equals(model_name));
                if (sysma != null)
                {
                    return Content("false");
                }
                else
                {
                    return Content("true");
                }
            }
            else
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
            SYSMA sysma = DBHelper.SYSMA.FirstOrDefault(ma => ma.ID.Equals(info_id));
            if (sysma != null)
            {

                //删除当前模块关联
                var sysmbs = DBHelper.SYSMB.Where(mb => mb.MB001.Equals(sysma.ID));
                if (sysmbs != null && sysmbs.Count() > 0)
                {
                    foreach (var item in sysmbs)
                    {
                        var userbs = DBHelper.USERB.Where(rb => rb.RB002.Equals(item.ID));
                        DBHelper.USERB.DeleteAllOnSubmit(userbs);
                        //删除角色权限分配
                        DBHelper.SubmitChanges();
                    }
                    //删除权限功能
                    DBHelper.SYSMB.DeleteAllOnSubmit(sysmbs);
                    DBHelper.SubmitChanges();
                }
                //删除模块
                DBHelper.SYSMA.DeleteOnSubmit(sysma);
                DBHelper.SubmitChanges();
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }



        /// <summary>
        /// 下单信息编辑时获取根菜单，特注：暂时支持两级遍历
        /// </summary>
        /// <returns>ID,菜单名,父级ID</returns>
        public ActionResult GetMenuList()
        {
            var sysmas = DBHelper.SYSMA.Where(ma => ma.MA007.Equals(true) && ma.MA005.Equals(-1));
            if (sysmas != null)
            {
                var MenuList = sysmas.Select(ma => new { id = ma.ID, text = ma.MA001, children = DBHelper.SYSMA.Where(m => m.MA007.Equals(true) && m.MA005.Equals(ma.ID)).Select(mm => new { id = mm.ID, text = mm.MA001 }) });
                //GetJsTree(-1);
                return ReturnJson(MenuList);
            }
            else
            { return null; }
        }

        List<JsTree> MenuList = new List<JsTree>();
        List<JsTree> ChildTree = new List<JsTree>();
        public void GetJsTree(int Pid)
        {
            var sysmas = DBHelper.SYSMA.Where(ma => ma.MA007.Equals(true) && ma.MA005.Equals(Pid));

            ChildTree.Clear();
            foreach (var sysma in sysmas)
            {
                JsTree newJsTree = new JsTree();
                newJsTree.id = sysma.ID;
                newJsTree.text = sysma.MA001;
                GetJsTree(sysma.ID);
                ChildTree.Add(newJsTree);
                newJsTree.children = ChildTree;
                MenuList.Add(newJsTree);
            }
        }

    }

    public class JsTree
    {
        public int? id;
        public string text;
        public List<JsTree> children;
    }

}