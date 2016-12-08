using System.Web;
using System.Web.Optimization;

namespace Net.HostServer
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/netserver").Include(
						"~/Scripts/searchhome.js",
						"~/Scripts/jquery.easypiechart.js",
						"~/Scripts/pace/pace.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/ace").Include(
				  "~/Scripts/ace/ace-elements.min.js",
				  "~/Scripts/ace/ace-extra.min.js",
				  "~/Scripts/ace/ace.min.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/css/font-awesome.css",
					  "~/css/pace/dataurl.css",
					  "~/Content/site.css",
					  "~/Content/search.css"));

			bundles.Add(new StyleBundle("~/Content/ace").Include(
				  "~/css/ace/ace.min.css",
				  "~/css/ace/ace-rtl.min.css",
				  "~/css/ace/ace-skins.min.css",
				  "~/css/ace/ace-ie.min.css"));
		}
	}
}
