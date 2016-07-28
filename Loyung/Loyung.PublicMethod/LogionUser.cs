using Loyung.DBModels;
/*
 * 创建时间：2016-06-28
 * 创建人：刘自洋
 * 说明：用户登出操作类
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyung.PublicMethod
{
    public class LogionUser : BaseController
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码(明文)</param>
        /// <returns>用户资料，为null则登录失败</returns>
        public  Userinfo Logion(string UserName, string Password)
        {
            Userinfo user=new global::Userinfo() ;
            var useub = DBHelper.USEUB.FirstOrDefault(ub => ub.UB002.Equals(UserName) && ub.UB003.Equals(Security.EncryptMd5(Password)));
            if (useub != null)
            {
                user.RealName = useub.USEUA.UA003;
                user.RoleId = useub.USEUA.USERC.FirstOrDefault().RC002.Value;
                user.RoleName = useub.USEUA.USERC.FirstOrDefault().USERA.RA001;
                user.UserId = useub.UB001.Value;
                user.UserName = useub.UB002;
            }
            return user;
        }

    }
}

/// <summary>
/// 用户信息
/// </summary>
public class Userinfo
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public Int64 UserId;
    /// <summary>
    /// 用户登录名
    /// </summary>
    public string UserName;
    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName;
    /// <summary>
    /// 用户角色名称
    /// </summary>
    public string RoleName;
    /// <summary>
    /// 用户角色ID
    /// </summary>
    public int RoleId;
}
