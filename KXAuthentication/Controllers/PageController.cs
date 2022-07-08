using CMS.DocumentEngine.Types.KXSamples;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using KXAuthentication.Controllers;
using KXAuthentication.Models.Page;

[assembly: RegisterPageRoute(Page.CLASS_NAME, typeof(PageController))]
namespace KXAuthentication.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;

        public PageController(IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
        }

        public IActionResult Index()
        {
            var page = pageDataContextRetriever.Retrieve<Page>().Page;

            return View(new PageViewModel(page));
        }
    }
}
