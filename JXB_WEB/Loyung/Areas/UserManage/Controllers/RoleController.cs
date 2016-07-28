/*
 * 创建时间：2016-06-20
 * 创建人：刘自洋
 * 说明：该模块用于用户管理——系统角色管理
 */
using Loyung.DBModels;
using Loyung.PublicMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loyung.Areas.UserManage.Controllers
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    public class RoleController : BaseController
    {
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleList(int? page, int? take)
        {
            page = page ?? 1;
            take = take ?? 2;
            var useras = DBHelper.USERA.Where(da => true);
            ViewBag.TotalPage = Math.Ceiling(((double)useras.Count() / (double)take));
            ViewBag.CurrentPage = page;
            //条件查询
            if (!string.IsNullOrWhiteSpace(Request.Form["search_RA001"]))
            {
                useras = useras.Where(ra => ra.RA001.Contains(Request.Form["search_RA001"]));
                ViewBag.TotalPage = Math.Ceiling(((double)useras.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (useras != null)
            {
                useras = useras.Skip((int)take * ((int)page - 1)).Take((int)take);
                return View(useras);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="take">页面显示数据条数</param>
        /// <returns></returns>
        public ActionResult GetRoleList(int? page, int? take)
        {
            page = page ?? 1;
            take = take ?? 2;
            var useras = DBHelper.USERA.Where(da => true);
            ViewBag.TotalPage = Math.Ceiling(((double)useras.Count() / (double)take));
            ViewBag.CurrentPage = page;
            //条件查询
            if (!string.IsNullOrWhiteSpace(Request.Form["search_RA001"]))
            {
                useras = useras.Where(ra => ra.RA001.Contains(Request.Form["search_RA001"]));
                ViewBag.TotalPage = Math.Ceiling(((double)useras.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }

            var PageUseras = useras.Skip((int)take * ((int)page - 1)).Take((int)take);
            //如果本页没有数据直接显示前一页
            if (page > 1 && useras.Skip((int)take * ((int)page - 1)).Take((int)take).Count() < 1)
            {
                ViewBag.CurrentPage = Math.Ceiling(((double)useras.Count() / (double)take));
                PageUseras = useras.Skip((int)take * ((int)page - 2)).Take((int)take);
            }
            return PartialView(PageUseras);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        public ActionResult GetRoleInfo(int id)
        {
            var UseRInfo = DBHelper.USERA.FirstOrDefault(da => da.ID.Equals(id));
            if (UseRInfo != null)
            {
                return ReturnJson(new { UseRInfo.RA001, UseRInfo.RA002});
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        public ActionResult DeleteRoleInfo(int id)
        {
            var RoleInfo = DBHelper.USERA.FirstOrDefault(da => da.ID.Equals(id));
            if (RoleInfo != null)
            {
                //查找角色关联权限
                var userbs = DBHelper.USERB.Where(rb => rb.RB001.Equals(RoleInfo.ID));
                //查找角色关联用户
                var usercs = DBHelper.USERC.Where(rc => rc.RC002.Equals(RoleInfo.ID));
                if (userbs.Count()<=0 && usercs.Count()<=0)
                {
                    DBHelper.USERA.DeleteOnSubmit(RoleInfo);
                    DBHelper.SubmitChanges();
                    return Content("true");
                }
                else
                {
                    return Content("false");
                 }
            }
            else
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult AddInfo()
        {
            try
            {
                USERA usera = null;
                if (Request.Form["info_id"] == "#")
                {
                    usera = DBHelper.USERA.FirstOrDefault(ra => ra.RA001.Equals(Request.Form["RA001"]) || ra.RA002.Equals(Request.Form["RA002"]));
                    if (usera == null)
                    {
                        usera = new USERA();
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    usera = DBHelper.USERA.FirstOrDefault(da => da.ID.Equals(Request.Form["info_id"]));
                }
                usera.RA001 = Request.Form["RA001"];
                usera.RA002 = Request.Form["RA002"];
                if (Request.Form["info_id"] == "#")
                    DBHelper.USERA.InsertOnSubmit(usera);
                DBHelper.SubmitChanges();
                return Content("true");

            }
            catch
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public ActionResult RoleDetail(int id)
        {
            var usera = DBHelper.USERA.FirstOrDefault(ra => ra.ID.Equals(id));
            //获取全部已通过根模块
            ViewBag.sysmaRoot = DBHelper.SYSMA.Where(ma => ma.MA005.Value.Equals(-1) && ma.MA007.Equals(true));
            //获取全部已通过子模块
            ViewBag.sysmas = DBHelper.SYSMA.Where(ma => ma.MA007.Equals(true) && ma.MA005.Value != -1);
            //获取所有已通过功能
            ViewBag.sysmbs = DBHelper.SYSMB.Where(mb => mb.MB004.Equals(true));
            //获取当前用户拥有权限的功能
            ViewBag.userbs = DBHelper.USERB.Where(rb => rb.RB001.Equals(id));
            return View(usera);
        }

        /// <summary>
        /// 获取根模块，绑定下拉列表
        /// </summary>
        /// <returns>value:模块编号,text:模块名称</returns>
        public ActionResult GetRootModel()
        {
            var sysmas = DBHelper.SYSMA.Where(ma => ma.MA007.Equals(true) && ma.MA005.Value.Equals(-1)).OrderBy(or => or.MA004);
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
        /// 修改角色权限
        /// </summary>
        /// <returns></returns>
        public ActionResult EditRoleLimit()
        {
            if (Request["sysmbId"] != null && Request["RoleId"]!=null)
            {
                string RoleId = Request["RoleId"];
                string[] sysmbId = Request["sysmbId"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sysmbId.Length; i++)
                { 
                    //添加新增权限
                    var userb = DBHelper.USERB.FirstOrDefault(rb => rb.RB002.Equals(sysmbId[i]) && rb.RB001.Equals(RoleId));
                    if (userb ==null)
                    {
                        USERB NewUserb = new USERB();
                        NewUserb.RB001 = int.Parse(RoleId);
                        NewUserb.RB002 = int.Parse(sysmbId[i]);
                        DBHelper.USERB.InsertOnSubmit(NewUserb);
                        DBHelper.SubmitChanges();
                    }
                }
                //删除已选择移除权限
                var userbs = DBHelper.USERB.Where(rb =>rb.RB001.Equals(RoleId));
                foreach (var item in userbs)
                {
                    int deleteId = Array.IndexOf<string>(sysmbId, item.RB002.ToString());
                    if (deleteId==-1)
                   {
                       var DeeleteUserb = DBHelper.USERB.FirstOrDefault(rb => rb.RB002.Equals(item.RB002) && rb.RB001.Equals(RoleId));
                       if (DeeleteUserb != null)
                       {
                           DBHelper.USERB.DeleteOnSubmit(DeeleteUserb);
                           DBHelper.SubmitChanges();
                       }
                   }
                }
                return Content("true");

            }
            else
            {
                return Content("false");
            }
        }

	}
}