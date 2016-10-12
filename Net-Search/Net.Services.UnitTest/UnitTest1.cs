using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Models;

namespace Net.Services.UnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void CreateSitePage()
		{
			Manager manager = new Manager();

			SitePage sitePage = new SitePage();
			sitePage.Id = 123456;
			sitePage.Title = "Hello";
			sitePage.Description = "Description";
			sitePage.Url = "http://www.ac.com";

			sitePage.Content = sitePage.Description;
			sitePage.CreatedTimeStamp = System.DateTime.Now;
			sitePage.ModifiedTimeStamp = System.DateTime.Now;
			var isSuccessed = manager.CreateSitePage(sitePage);
			Assert.IsTrue(isSuccessed);
		}

		[TestMethod]
		public void GetSitePages()
		{
			Manager manager = new Manager();

			var isSuccessed = manager.SelectAllSitePage();
		}
	}
}