using System;
using Net.Api;
using Net.Models;
using Net.Server.ServiceReference;
using Net.Utils;
using Net.Utils.Common;

namespace Net.Server
{
	public partial class link : System.Web.UI.Page
	{
		private static Manager manager = new Manager();
		protected string url { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			#region Handle parameter

			try
			{
				var client = new ManagerClient();

				url = Request["url"];
				if (string.IsNullOrWhiteSpace(url))
				{
					return;
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

				Response.Write(string.Format("<script language='javascript'>window.location='{0}'</script>", url));
			}
			catch (Exception ex)
			{
				Log.Error("link.Page_Load", ex);
			}

			#endregion Handle parameter
		}
	}
}