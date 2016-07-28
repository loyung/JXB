/*
 * 创建时间：2016-06-28
 * 创建人：刘自洋
 * 说明：绑定UI数据格式化
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 对页面列表数据显示格式化
/// </summary>
   public static class BindFormat
    {
       /// <summary>
        /// 对返回的Boolean类型进行格式化显示
       /// </summary>
        /// <param name="value">Boolean值可为null</param>
       /// <param name="defaultvalue">为null时显示值</param>
       /// <param name="truevalue">为true时显示值</param>
       /// <param name="falsevalue">为false时显示值</param>
       /// <returns></returns>
       public static string BooleanFormat(bool? value,string defaultvalue,string truevalue,string falsevalue)
       {
           if (value == null)
           {
               return defaultvalue;
           }
           if (value.Value)
           {
               return truevalue;
           }
           else
           {
               return falsevalue;
           }
       }
    }
