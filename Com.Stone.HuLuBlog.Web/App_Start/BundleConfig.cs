using System.Web;
using System.Web.Optimization;

namespace Com.Stone.HuLuBlog.Web
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/layui").Include(
            "~/layui/layui.js"));
            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                "~/Scripts/global.js","~/Scripts/cookie.js","~/Scripts/GetDataByJson.js"));


            bundles.Add(new StyleBundle("~/Content/layui").Include(
                       "~/layui/css/layui.css", "~/layui/css/theme10.min.css"));
            bundles.Add(new StyleBundle("~/Content/fontAwesome").Include(
                "~/Content/font-awesome/css/font-awesome.min.css"));
            bundles.Add(new StyleBundle("~/Content/global").Include(
                "~/Content/global.css"));
        }
    }
}
