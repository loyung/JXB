using Loyung.DBModels;
using Loyung.PublicMethod;
/*
 * 创建时间：2016-06-21
 * 创建人：刘自洋
 * 说明：该模块用于对用户进行管理
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loyung.Areas.UserManage.Controllers
{
    /// <summary>
    /// 用户列表管理
    /// </summary>
    public class UserManageController : BaseController
    {

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList(int? page, int? take)
        {
            page = page ?? 1;
            take = take ?? 2;
            var useuas = DBHelper.USEUA.Where(ua => true && ua.UA012 != true && ua.UA100 != false);
            ViewBag.TotalPage = Math.Ceiling(((double)useuas.Count() / (double)take));
            ViewBag.CurrentPage = page;
            //条件查询
            if (!string.IsNullOrWhiteSpace(Request.Form["search_UA001"]))
            {
                useuas = useuas.Where(ua => ua.UA001.Contains(Request.Form["search_UA001"]) || ua.UA002.Contains(Request.Form["search_UA001"]) || ua.UA003.Contains(Request.Form["search_UA001"]));
                ViewBag.TotalPage = Math.Ceiling(((double)useuas.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }
            if (useuas != null)
            {
                useuas = useuas.Skip((int)take * ((int)page - 1)).Take((int)take);
                return View(useuas);
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
        public ActionResult GetUserList(int? page, int? take)
        {
            page = page ?? 1;
            take = take ?? 2;
            var useuas = DBHelper.USEUA.Where(ua => true && ua.UA012 != true && ua.UA100 != false);
            ViewBag.TotalPage = Math.Ceiling(((double)useuas.Count() / (double)take));
            ViewBag.CurrentPage = page;
            //条件查询
            if (!string.IsNullOrWhiteSpace(Request.Form["search_UA001"]))
            {
                useuas = useuas.Where(ua => ua.UA001.Contains(Request.Form["search_UA001"]) || ua.UA002.Contains(Request.Form["search_UA001"]) || ua.UA003.Contains(Request.Form["search_UA001"]));
                ViewBag.TotalPage = Math.Ceiling(((double)useuas.Count() / (double)take));
                ViewBag.CurrentPage = page = 1;
            }

            var PageUseras = useuas.Skip((int)take * ((int)page - 1)).Take((int)take);
            //如果本页没有数据直接显示前一页
            if (page > 1 && useuas.Skip((int)take * ((int)page - 1)).Take((int)take).Count() < 1)
            {
                ViewBag.CurrentPage = Math.Ceiling(((double)useuas.Count() / (double)take));
                PageUseras = useuas.Skip((int)take * ((int)page - 2)).Take((int)take);
            }
            return PartialView(PageUseras);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public ActionResult DeleteUserInfo(int id)
        {
            var UserInfo = DBHelper.USEUA.FirstOrDefault(da => da.ID.Equals(id));
            if (UserInfo != null)
            {
                //查找用户角色
                var userbs = DBHelper.USERC.Where(rc => rc.RC001.Equals(UserInfo.ID));
                //查找用户账户信息
                var useubs = DBHelper.USEUB.Where(ub => ub.UB001.Equals(UserInfo.ID));

                if (userbs.Count() > 0 && useubs.Count() > 0)
                {
                    DBHelper.USERC.DeleteAllOnSubmit(userbs);
                    DBHelper.USEUB.DeleteAllOnSubmit(useubs);
                    DBHelper.USEUA.DeleteOnSubmit(UserInfo);
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
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUserInfo()
        {
            try
            {
                USEUA useua = null;
                if (Request.Form["info_id"] == "#")
                {
                    useua = DBHelper.USEUA.FirstOrDefault(ra => ra.UA001.Equals(Request.Form["UA001"]));
                    if (useua == null)
                    {
                        useua = new USEUA();
                    }
                }
                else
                {
                    useua = DBHelper.USEUA.FirstOrDefault(da => da.ID.Equals(Request.Form["info_id"]));
                }
                useua.UA001 = Request.Form["UA001"];
                useua.UA002 = Request.Form["UA002"];
                if (!string.IsNullOrWhiteSpace(Request.Form["UA003"]))
                useua.UA003 = Request.Form["UA003"];
                useua.UA004 = Request.Form["UA004"];
                if (!string.IsNullOrWhiteSpace(Request.Form["UA005"]))
                useua.UA005 = Convert.ToDateTime(Request.Form["UA005"]);
                useua.UA006 = Request.Form["UA006"];
                useua.UA007 = Request.Form["UA007"];
                useua.UA008 = Request.Form["UA008"];
                useua.UA009 = Request.Form["UA009"];
                useua.UA010 = Request.Form["UA010"];
                useua.UA011 = DateTime.Now;
                useua.UA012 = false; 
                useua.UA100 = true;
                if (Request.Form["info_id"] == "#")
                {
                    //用户角色信息
                    USERC userc = new USERC();
                    userc.RC002 = int.Parse(Request.Form["RC002"]);
                    useua.USERC.Add(userc);
                    //用户账户信息添加
                    USEUB useub = new USEUB();
                    useub.UB002 = useua.UA001;
                    useub.UB003 = Security.EncryptMd5(Request["UB003"]);
                    useua.USEUB.Add(useub);
                    DBHelper.USEUA.InsertOnSubmit(useua);
                }
                //用户账户信息修改
                USEUB Edituseub = useua.USEUB.FirstOrDefault();
                Edituseub.UB002 = useua.UA001;
                Edituseub.UB003 = Security.EncryptMd5(Request["UB003"]);
                useua.USEUB.Add(Edituseub);
                //用户角色信息修改
                USERC Edituserc = useua.USERC.FirstOrDefault();
                Edituserc.RC002 = int.Parse(Request.Form["RC002"]); ;
                useua.USERC.Add(Edituserc);
                DBHelper.SubmitChanges();
                return Content("true");
            }
            catch
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 获取绑定用户信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public ActionResult GetUserInfo(int id)
        {
            var UseuaInfo = DBHelper.USEUA.FirstOrDefault(ua => ua.ID.Equals(id));
            if (UseuaInfo != null)
            {
                return ReturnJson(new {
                    UseuaInfo.UA001,
                    UseuaInfo.UA002,
                    UseuaInfo.UA003,
                    UseuaInfo.UA004,
                    UseuaInfo.UA005,
                    UseuaInfo.UA006,
                    UseuaInfo.UA007,
                    UseuaInfo.UA008,
                    UseuaInfo.UA009,
                    UseuaInfo.UA010,
                    UseuaInfo.UA012,//是否冻结
                    UseuaInfo.USERC.FirstOrDefault().RC002,//用户角色
                });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 用户信息页面加载
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            return View();
        }

        /// <summary>
        /// 获取所有角色，绑定下拉列表
        /// </summary>
        /// <returns>value:角色编号,text:角色名称</returns>
        public ActionResult GetAllRole()
        {
            var useras = DBHelper.USERA.Where(ra => true);
            if (useras != null)
            {
                return ReturnJson(useras.Select(se => new { value = se.ID, text = se.RA001 }));
            }
            else
            {
                return null;
            }
        }



    }
}