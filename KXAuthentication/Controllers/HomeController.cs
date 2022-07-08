using CMS.DocumentEngine.Types.KXSamples;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using KXAuthentication.Controllers;
using KXAuthentication.Models.Home;

[assembly: RegisterPageRoute(HomePage.CLASS_NAME, typeof(HomeController))]
namespace KXAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;

        public HomeController(IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
        }

        public IActionResult Index()
        {
            var page = pageDataContextRetriever.Retrieve<HomePage>().Page;

            return View(new HomeViewModel(page));
        }
    }
}
