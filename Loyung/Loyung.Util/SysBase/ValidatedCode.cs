/*
 * 创建时间：2016-04-27
 * 创建人：刘自洋
 * 说明：用于必要输入验证时调用
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace Loyung.Util.SysBase
{
    /// <summary>  
    /// 验证码生成类
    /// </summary>  
    public class ValidatedCode
    {
        #region  验证码长度(默认4个验证码的长度)
        int length = 4;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认20像素，可以自行修改)
        int fontSize = 25;
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        int padding = 2;
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }
        #endregion

        #region 是否生成干扰线
        bool lines = true;
        public bool Lines
        {
            get { return lines; }
            set { lines = value; }
        }
        #endregion

        #region 干扰线颜色是否随机
        bool isRandomLineColor = true;

        public bool IsRandomLineColor
        {
            get { return isRandomLineColor; }
            set { isRandomLineColor = value; }
        }

        #endregion

        #region 输出干扰线的颜色(默认灰色)
        Color linesColor = Color.Black;
        public Color LinesColor
        {
            get { return linesColor; }
            set { linesColor = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        bool chaos = true;
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color chaosColor = Color.MidnightBlue;
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }
        #endregion

        #region 燥点的颜色是否随机
        bool isRandomChaosColor = true;
        public bool IsRandomChaosColor
        {
            get { return isRandomChaosColor; }
            set { isRandomChaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        Color[] colors = { Color.LightSalmon, Color.LightSeaGreen, Color.LightSkyBlue, Color.LimeGreen, Color.Blue, Color.Magenta, Color.Brown, Color.Crimson, Color.MidnightBlue, Color.MediumVioletRed, Color.DeepPink, Color.Gold, Color.Thistle, Color.LawnGreen };
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        #endregion

        #region 自定义字体数组
        string[] fonts = { "Arial", "Georgia" };
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }
        #endregion

        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>  
        /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）  
        /// </summary>  
        /// <param name="srcBmp">图片路径</param>  
        /// <param name="bXDir">如果扭曲则选择为True</param>  
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>  
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>  
        /// <returns></returns>  
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色  
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色  
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }



        #endregion

        #region 生成随机字符码
        public string CreateVerifyCode(int codeLen)
        {
            Random CheckNum = new Random();

            string code = "";
            string[] codes = CodeSerial.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < codeLen; i++)
            {
                code += codes[CheckNum.Next(0, codes.Length-1)];
                //按指定规则显示Code
                //if (i % 2 == 0)
                //    code += (Convert.ToChar(CheckNum.Next(97, 122))).ToString();
                //else
                //    code += CheckNum.Next(0, 9).ToString();
            }

            return code;
        }
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(4);
        }
        #endregion

        #region 生成校验码图片
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(BackgroundColor);

            Random rand = new Random();

            //给背景添加随机生成的燥点  
            if (this.Chaos)
            {
                 Pen pen=new Pen (ChaosColor,0);
                int c = Length * 20;

                for (int i = 0; i < c; i++)
                {
                    if (isRandomChaosColor)
                        pen.Color = Colors[rand.Next(Colors.Length - 1)];
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }
            //给背景添加随机生成干扰线
            if (this.Lines)
            {
                for (int i = 0; i < 3; i++)
                {
                    int x1 = rand.Next(image.Width);
                    int x2 = rand.Next(image.Width);
                    int y1 = rand.Next(image.Height);
                    int y2 = rand.Next(image.Height);
                    x1 = x1 - (image.Width / 2);
                    y1 = y1 - (image.Height / 2);
                    x2 = x2 + (image.Width / 2);
                    y2 = y2 + (image.Height / 2);
                    if (isRandomLineColor)
                        g.DrawLine(new Pen(Colors[rand.Next(Colors.Length - 1)],3f), x1, y1, x2, y2);
                    else
                        g.DrawLine(new Pen(LinesColor, 3f), x1, y1, x2, y2);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //随机字体和颜色的验证码字符  
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro  
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形（Add By 51aspx.com）  
            image = TwistImage(image, true, 8, 4);

            return image;
        }
        #endregion

        #region 生成校验图片输出到页面
        /// <summary>
        /// 将创建好的图片输出到页面
        /// </summary>
        /// <param name="code">生成验证码Code</param>
        /// <param name="context">页面数据上下文</param>
        public void CreateImageOnPage(string code, HttpContext context)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = this.CreateImageCode(code);

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            context.Response.ClearContent();
            context.Response.ContentType = "image/Jpeg";
            context.Response.BinaryWrite(ms.GetBuffer());

            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }       
        #endregion

        #region 生成Byte[]验证图片
        /// <summary>
        /// 生成Byte[]验证图片
        /// </summary>
        /// <param name="code">创建图片Code</param>
        /// <returns>图片格式的Byte数组</returns>
        public byte[] CreateImage(string code)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Bitmap image = this.CreateImageCode(code);
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        #endregion
    }
}
