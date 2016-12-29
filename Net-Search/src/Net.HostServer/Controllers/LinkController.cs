using System;
using System.Linq;
using System.Web.Mvc;
using Net.HostServer.ServiceReference;
using Net.Models;
using Net.Utils;
using Net.Utils.Common;

namespace Net.HostServer.Controllers
{
	public class LinkController : Controller
	{
		// GET: Link
		public ActionResult Index(string url)
		{
			#region Handle parameter

			try
			{
				var client = new ManagerClient();

				if (string.IsNullOrWhiteSpace(url))
				{
					return null;
				}
				if (url.IndexOf("http") == -1)
				{
					url = "http://" + url;
				}

				Linked linked = new Linked();
				linked.IP = HttpHelper.GetIp();
				linked.Url = url;
				linked.CreatedTimeStamp = System.DateTime.Now;

				var isSuccessed = client.CreateSiteClickedLink(linked);
				var single = client.SelectSitePage(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), url).FirstOrDefault();
				if (single != null)
				{
					single.Linked = single.Linked + 1;
					client.UpdateSitePage(single);
				}
				Response.Write(string.Format("<script language='javascript'>window.location='{0}'</script>", url));
			}
			catch (Exception ex)
			{
				Log.Error("link.Page_Load", ex);
			}

			#endregion Handle parameter

			return View();
		}
	}
}