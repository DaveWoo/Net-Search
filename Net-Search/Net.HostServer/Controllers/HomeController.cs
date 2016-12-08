using System.Web.Mvc;
using Net.HostServer.ServiceReference;
using Net.Utils;

namespace Net.HostServer.Controllers
{
	public class HomeController : Controller
	{
		private static ManagerClient client = null;

		static HomeController()
		{
			client = new ManagerClient();
		}

		public ActionResult Index(string q)
		{
			ViewBag.SiteName = Constants.SITE_NAME;
			if (!string.IsNullOrWhiteSpace(q))
			{
				string url = Request.Path + @"\\s?q=" + q;
				Response.Write(string.Format("<script language='javascript'>window.location='{0}'</script>", url));
			}
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}