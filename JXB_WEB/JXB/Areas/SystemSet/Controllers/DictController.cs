/*
 * 创建时间：2016-05-26
 * 创建人：刘自洋
 * 说明：该模块用于管理系统设置——字典管理模块
 */
using Loyung.DBModels;
using Loyung.PublicMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loyung.Areas.SystemSet.Controllers
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public class DictController : BaseController
    {

        /// <summary>
        /// 字典类别
        /// </summary>
        /// <returns></returns>
        public ActionResult DictList(int? page, int? take)
        {
            page = page ?? 1;
            take = take ?? 2;
            var sysdas = DBHelper.SYSDA.Where(da => true);
            ViewBag.TotalPage = Math.Ceiling(((double)sysdas.Count() / (double)take));
            ViewBag.CurrentPage = page;
            //条件查询
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DA002"]))
            {
                sysdas = sysdas.Where(sa => sa.DA002.Contains(Request.Form["search_DA002"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdas.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DA001"]))
            {
                sysdas = sysdas.Where(sa => sa.DA001.Contains(Request.Form["search_DA001"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdas.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (sysdas != null)
            {
                sysdas = sysdas.Skip((int)take * ((int)page - 1)).Take((int)take);
                return View(sysdas);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 分页获取字典列表
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="take">页面显示数据条数</param>
        /// <returns></returns>
        public ActionResult GetDictList(int? page, int? take)
        {
            page = page ?? 1;
            take = take ?? 2;
            var sysdas = DBHelper.SYSDA.Where(da => true);
            ViewBag.TotalPage = Math.Ceiling(((double)sysdas.Count() / (double)take));
            ViewBag.CurrentPage = page;
            //条件查询
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DA002"]))
            {
                sysdas = sysdas.Where(sa => sa.DA002.Contains(Request.Form["search_DA002"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdas.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DA001"]))
            {
                sysdas = sysdas.Where(sa => sa.DA001.Contains(Request.Form["search_DA001"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdas.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }

            var PageSysdas = sysdas.Skip((int)take * ((int)page - 1)).Take((int)take);
            //如果本页没有数据直接显示前一页
            if (page > 1 && sysdas.Skip((int)take * ((int)page - 1)).Take((int)take).Count() < 1)
            {
                ViewBag.CurrentPage = Math.Ceiling(((double)sysdas.Count() / (double)take));
                PageSysdas = sysdas.Skip((int)take * ((int)page - 2)).Take((int)take);
            }
            return PartialView(PageSysdas);
        }

        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        public ActionResult GetDictInfo(int id)
        {
            var DictInfo = DBHelper.SYSDA.FirstOrDefault(da => da.ID.Equals(id));
            if (DictInfo != null)
            {
                return ReturnJson(new { DA001 = DictInfo.DA001, DA002 = DictInfo.DA002, DA003 = DictInfo.DA003 });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除字典信息
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        public ActionResult DeleteDictInfo(int id)
        {
            var DictInfo = DBHelper.SYSDA.FirstOrDefault(da => da.ID.Equals(id));
            if (DictInfo != null)
            {
                //删除字典关联明细
                var sysdbs = DBHelper.SYSDB.Where(db => db.DB001.Equals(DictInfo.ID));
                DBHelper.SYSDB.DeleteAllOnSubmit(sysdbs);
                DBHelper.SYSDA.DeleteOnSubmit(DictInfo);
                DBHelper.SubmitChanges();
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 添加字典类别
        /// </summary>
        /// <returns></returns>
        public ActionResult AddInfo()
        {
            try
            {
                SYSDA sysda = null;
                if (Request.Form["info_id"] == "#")
                {
                    sysda = DBHelper.SYSDA.FirstOrDefault(da => da.DA001.Equals(Request.Form["DA001"]) || da.DA002.Equals(Request.Form["DA002"]));
                    if (sysda == null)
                    {
                        sysda = new SYSDA();
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    sysda = DBHelper.SYSDA.FirstOrDefault(da => da.ID.Equals(Request.Form["info_id"]));
                }
                sysda.DA001 = Request.Form["DA001"];
                sysda.DA002 = Request.Form["DA002"];
                sysda.DA003 = Request.Form["DA003"];
                if (Request.Form["info_id"] == "#")
                    DBHelper.SYSDA.InsertOnSubmit(sysda);
                DBHelper.SubmitChanges();
                return Content("true");

            }
            catch
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 字典明细列表获取
        /// </summary>
        /// <param name="id">字典类别ID</param>
        /// <param name="page">当前页</param>
        /// <param name="take">当前页显示数量</param>
        /// <returns></returns>
        public ActionResult DictDetailList(int id, int? page, int? take)
        {
            page = page == null ? 1 : page;
            take = take == null ? 2 : take;
            var sysdbs = DBHelper.SYSDB.Where(db => db.DB001.Equals(id));
            ViewBag.DA001 = DBHelper.SYSDA.FirstOrDefault(da => da.ID.Equals(id)).DA001;
            ViewBag.Pid = id;
            ViewBag.TotalPage = Math.Ceiling(((double)sysdbs.Count() / (double)take));
            ViewBag.CurrentPage = page;
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DB002"]))
            {
                sysdbs = sysdbs.Where(db => db.DB002.Contains(Request.Form["search_DB002"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdbs.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DB003"]))
            {
                sysdbs = sysdbs.Where(db => db.DB003.Contains(Request.Form["search_DB003"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdbs.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (sysdbs != null)
            {
                sysdbs = sysdbs.Skip((int)take * ((int)page - 1)).Take((int)take);
                return View(sysdbs);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 无刷新分页
        /// </summary>
        /// <param name="id">字典类别ID</param>
        /// <param name="page">当前页</param>
        /// <param name="take">当前页显示数量</param>
        /// <returns></returns>
        public ActionResult GetDictDetailList(int id, int? page, int? take)
        {
            page = page == null ? 1 : page;
            take = take == null ? 2 : take;
            ViewBag.Pid = id;
            var sysdbs = DBHelper.SYSDB.Where(db => db.DB001.Equals(id));
            ViewBag.TotalPage = Math.Ceiling(((double)sysdbs.Count() / (double)take));
            ViewBag.CurrentPage = page;
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DB002"]))
            {
                sysdbs = sysdbs.Where(db => db.DB002.Contains(Request.Form["search_DB002"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdbs.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["search_DB003"]))
            {
                sysdbs = sysdbs.Where(db => db.DB003.Contains(Request.Form["search_DB003"]));
                ViewBag.TotalPage = Math.Ceiling(((double)sysdbs.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            var PageSysdbs = sysdbs.Skip((int)take * ((int)page - 1)).Take((int)take);
            //如果本页没有数据直接显示前一页
            if (page > 1 && sysdbs.Skip((int)take * ((int)page - 1)).Take((int)take).Count() < 1)
            {
                ViewBag.CurrentPage = page - 1;
                PageSysdbs = sysdbs.Skip((int)take * ((int)page - 2)).Take((int)take);
            }
            return PartialView(PageSysdbs);
        }

        /// <summary>
        /// 添加字典明细
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDetailInfo()
        {
            try
            {
                SYSDB sysdb = null;
                if (Request.Form["info_id"] == "#")
                {
                    sysdb = DBHelper.SYSDB.FirstOrDefault(db => db.DB001.Equals(Request.Form["DB001"]) && db.DB002.Equals(Request.Form["DA002"]));
                    if (sysdb == null)
                    {
                        sysdb = new SYSDB();
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    sysdb = DBHelper.SYSDB.FirstOrDefault(da => da.ID.Equals(Request.Form["info_id"]));
                }
                sysdb.DB001 = int.Parse(Request.Form["DB001"]);
                sysdb.DB003 = Request.Form["DB003"];
                sysdb.DB002 = Request.Form["DB002"];
                sysdb.DB004 = Request.Form["DB004"];
                if (Request.Form["info_id"] == "#")
                    DBHelper.SYSDB.InsertOnSubmit(sysdb);
                DBHelper.SubmitChanges();
                return Content("true");

            }
            catch
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 获取字典明细
        /// </summary>
        /// <param name="id">字典明细ID</param>
        /// <returns></returns>
        public ActionResult GetDictDetInfo(int id)
        {
            var DetInfo = DBHelper.SYSDB.FirstOrDefault(db => db.ID.Equals(id));
            if (DetInfo != null)
            {
                return ReturnJson(new { DB001 = DetInfo.DB001, DB002 = DetInfo.DB002, DB003 = DetInfo.DB003, DB004 = DetInfo.DB004 });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除字典明细
        /// </summary>
        /// <param name="id">字典明细记录</param>
        /// <returns></returns>
        public ActionResult DeleteDictDetInfo(int id)
        {
            var DetInfo = DBHelper.SYSDB.FirstOrDefault(db => db.ID.Equals(id));
            if (DetInfo != null)
            {
                DBHelper.SYSDB.DeleteOnSubmit(DetInfo);
                DBHelper.SubmitChanges();
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }

    }
}