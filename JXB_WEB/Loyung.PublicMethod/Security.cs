/*
 * 创建人：刘自洋
 * 创建时间：2016-05-16
 * 说明：此类作为框架综合加密类
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Loyung.PublicMethod
{
    /// <summary>
    /// 加密，解密，编码，解码
    /// </summary>
  public  class Security
    {
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="ConvertString">MD5加密的字符</param>
        /// <returns>32位小写字符</returns>
      public static string EncryptMd5(string ConvertString)
        {
            string StrEncode = "";
            MD5 md = MD5.Create();
            StrEncode = BitConverter.ToString(md.ComputeHash(Encoding.Default.GetBytes(ConvertString))).Replace("-", "").ToLower();
            return StrEncode;
        }

      /// <summary>
      /// Base64字符编码
      /// </summary>
      /// <param name="ConvertString">待编码的字符</param>
      /// <returns>Base64编码字符</returns>
      public static string EncodeBase64(string ConvertString)
      {
          return Convert.ToBase64String(Encoding.Default.GetBytes(ConvertString));
      }

      /// <summary>
      /// Base64字符解码
      /// </summary>
      /// <param name="result">待解码字符</param>
      /// <returns>Base64解码字符</returns>
      public static string DecodeBase64(string result)
      {
          return Encoding.Default.GetString(Convert.FromBase64String(result));
      }

    }
}
