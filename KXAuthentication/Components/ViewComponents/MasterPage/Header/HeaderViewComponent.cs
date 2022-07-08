using Microsoft.AspNetCore.Mvc;
namespace KXAuthentication.Components.ViewComponents.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        public HeaderViewComponent() { }

        public IViewComponentResult Invoke()
        {
            return View("~/Components/ViewComponents/MasterPage/Header/Default.cshtml");
        }
    }
}
