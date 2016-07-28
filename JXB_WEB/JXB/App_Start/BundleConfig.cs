using System.Web;
using System.Web.Optimization;

namespace Loyung
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862       
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region JS区
            //默认
            bundles.Add(new ScriptBundle("~/Script/Default").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery.form-3.51.0.js",
                        "~/Scripts/bootstrap.js",
                        "~/Content/SysBase/scripts/jqBindControl.js",
                        "~/Content/Assembly/poshytip/jquery.poshytip.js",
                        "~/Content/SysBase/scripts/jsTools.js",
                        "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/Script/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            //验证
            bundles.Add(new ScriptBundle("~/Script/validate").Include(
                       "~/Content/Assembly/jquery.validate/jquery.validate.min.js",
                       "~/Content/Assembly/jquery.validate/jquery.validate.tip.js",
                       "~/Content/Assembly/jquery.validate/jquery.validator.messages_zh.js",
                       "~/Content/Assembly/jquery.validate/jquery.validate-methods.js",
                       "~/Content/Assembly/jquery.validate/jquery.metadata.js"));
            //layui系列(弹窗,日期控件)
            bundles.Add(new ScriptBundle("~/Script/layer").Include(
                 "~/Content/Assembly/layer/layer.js",
                 "~/Content/Assembly/laydate/laydate.js"
                ));
            //分页
            bundles.Add(new ScriptBundle("~/Script/BootPaginator").Include(
               "~/Content/Assembly/paginator/bootstrap-paginator.js"
               ));
            #endregion

            #region CSS区
            //默认
            bundles.Add(new StyleBundle("~/Css/Default").Include(
                      "~/Content/Default/css/bootstrap.min.css",
                      "~/Content/Default/css/font-awesome.min.css",
                      "~/Content/Assembly/poshytip/tip-twitter/tip-twitter.css",//poshytip皮肤
                      "~/Content/SysBase/css/sysbase.css"));
            //注册
            bundles.Add(new StyleBundle("~/Css/Regist").Include(
                "~/Content/SysBase/css/Regist.css"
                ));
            #endregion
        }
    }
}
