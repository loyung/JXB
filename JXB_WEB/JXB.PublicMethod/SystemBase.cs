using Loyung.DBModels;
/*
 * 创建时间：2016-05-18
 * 创建人：刘自洋
 * 说明：存放框架必备的功用方法
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyung.PublicMethod
{
    /// <summary>
    /// 存放系统一些常用方法
    /// </summary>
    public class SystemBase :BaseController
    {
        /// <summary>
        /// 根据传入用户判断用户所使用的模块
        /// </summary>
        /// <param name="UserInfo">用户对象</param>
        /// <returns></returns>
        public  IEnumerable<SYSMA> GetUserRoleMenu(long UserId)
        {
            USEUA useua =DBHelper.USEUA.FirstOrDefault(ua => ua.ID.Equals(UserId));
            if (useua != null)
            {
                //用户角色分配兼顾多个角色
                var queryMB =DBHelper.SYSMB.WhereIn(mb=>mb.ID, DBHelper.USERB.WhereIn(rb => rb.RB001, DBHelper.USERC.Where(rc => rc.RC001.Equals(UserId)).Select(rc => rc.RC001).ToArray()).Select(rb=>rb.RB002).ToArray());
                var queryMAItem = DBHelper.SYSMA.OrderBy(ma => ma.MA004).WhereIn(ma => ma.ID, queryMB.DistinctBy(mai => mai.ID).Where(ma=>ma.SYSMA.MA007.Equals(true)).Select(mb => mb.MB001).ToArray());
                List<SYSMA> queryMARoot = new List<SYSMA>();
                foreach (var sysma in queryMAItem.DistinctBy(mai => mai.ID))
                { 
                    int Pid=sysma.MA005==null?0:(int)sysma.MA005;
                    foreach (var ma in DBHelper.SYSMA.Where(mait => mait.ID.Equals(Pid)&&mait.MA007.Equals(true)))
                    {
                        if (ma.MA005 == -1)
                        {
                            queryMARoot.AddRange(DBHelper.SYSMA.Where(con => con.ID.Equals(ma.ID)));
                        }
                        Pid = ma.MA005 == null ? 0 : (int)ma.MA005;
                    }
                }
                List<SYSMA> queryMenu = new List<SYSMA>();
                queryMenu.AddRange(queryMARoot.DistinctBy(dis => dis.ID));
                queryMenu.AddRange(queryMAItem);
                return queryMenu;
            }
            else
            {
                return null;
            }
        }



    }
}
