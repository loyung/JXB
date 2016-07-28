using Loyung.DBModels;
using Loyung.PublicMethod;
using Loyung.Util.SysBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loyung.Controllers
{
    /// <summary>
    /// 系统注册登录等基础操作
    /// </summary>
    public class SysBaseController : BaseController
    {

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Regist()
        {
            return View();
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns>图片对象</returns>
        public ActionResult GetValidateCode(string time)
        {
            ValidatedCode vCode = new ValidatedCode();
            string checkNum = vCode.CreateVerifyCode(4);
            Session["ValidatedCode"] = checkNum;
            byte[] bytes = vCode.CreateImage(checkNum);
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 校验用户输入验证码
        /// </summary>
        /// <returns>校验结果true|false</returns>
        public ActionResult CheckCode()
        {
            string validateC = Session["ValidatedCode"].ToString();
            string veryCode = Request["validateCode"].Trim();
            if (validateC.ToLower() == veryCode.ToLower())
                return Content("true");
            else
                return Content("false");
        }

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <returns>校验结果true可以注册|false已存在</returns>
        public ActionResult CheckUserName()
        {
            if (Request["userName"] != null)
            {
                USEUB useub = DBHelper.USEUB.FirstOrDefault(ub => ub.UB002.Equals(Request["userName"]));
                if (useub != null)
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
                return Content("true");
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns>添加结果</returns>
        public ActionResult AddUser()
        {
            USEUA useua = new USEUA();
            useua.UA001 = Request["inputUserName"].Trim();
            useua.UA006 = Request["inputMobile"];
            useua.UA008 = Request["inputEmail"];
            useua.UA011 = DateTime.Now;
            useua.UA012 = false;         
            USEUB useub=new USEUB ();
            useub.UB001=useua.ID;
            useub.UB002=Request["inputUserName"];
            useub.UB003=Security.EncryptMd5(Request["inputPwd"]);
            useua.USEUB.Add(useub);
            //DBHelper.USEUA.InsertOnSubmit(useua);
            //DBHelper.SubmitChanges();
            return Content("true");
        }

        /// <summary>
        /// 后台管理页加载
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            return View();
        }
        /// <summary>
        /// 登录页加载
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginUser()
        {

            if (Request.Form["LoginUserName"] != null && Request.Form["LoginPwd"] != null)
            {
               
                string LoginUserName = Request.Form["LoginUserName"];
                string LoginPwd =Request.Form["LoginPwd"];
                Userinfo  user= new LogionUser().Logion(LoginUserName, LoginPwd);
                if (user != null)
                {
                    Session["Userinfo"] = user;
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
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            if (Session["Userinfo"] != null)
            {
                this.Session.RemoveAll();
            }
            return this.Redirect("/SysBase/Login");
        }

    }
}